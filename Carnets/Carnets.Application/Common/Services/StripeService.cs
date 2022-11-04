using Carnets.Application.Consts;
using Carnets.Application.Enums;
using Carnets.Application.Interfaces;
using Carnets.Application.Models;
using Carnets.Domain.Models;
using Common.Exceptions;
using Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace Carnets.Application.Services
{
    public class StripeService : IPaymentService
    {
        private readonly ILogger<StripeService> _logger;

        private string WebhookSecret { get; init; }

        private string DefaultCurrency { get; init; }

        public StripeService(
            IConfiguration configuration,
            ILogger<StripeService> logger)
        {
            _logger = logger;
            DefaultCurrency = configuration.GetValue<string>("DefaultCurrency");
            WebhookSecret = configuration.GetValue<string>("WebhookSecret");
        }

        public async Task<string> GetOrCreateCustomer(string userId)
        {
            var customerService = new CustomerService();
            var listOptions = new CustomerListOptions
            {
                Limit = 1,
                Email = $"{userId}@muscleUp.com"
            };
            StripeList<Customer> customers = await customerService.ListAsync(listOptions);
            if (customers.Any())
            {
                return customers.First().Id;
            }

            var createOptions = new CustomerCreateOptions
            {
                Email = $"{userId}@muscleUp.com",
            };
            var customer = await customerService.CreateAsync(createOptions);

            return customer.Id;
        }

        public async Task CreateProduct(GympassType gympassType)
        {
            var options = new ProductCreateOptions
            {
                // will be used by stripe
                Id = gympassType.GympassTypeId,
                Name = gympassType.GympassTypeName,
                Active = true,
                DefaultPriceData = new ProductDefaultPriceDataOptions()
                {
                    Currency = DefaultCurrency,
                    UnitAmountDecimal = Convert.ToDecimal(gympassType.Price * 100),
                    Recurring = new ProductDefaultPriceDataRecurringOptions { 
                        Interval = gympassType.Interval.ToString().ToLower(),
                        IntervalCount = gympassType.IntervalCount
                    }
                },
                Description = string.IsNullOrEmpty(gympassType.Description) ? null : gympassType.Description,
            };
            var service = new ProductService();
            var product = await service.CreateAsync(options);

            if (product.Id != gympassType.GympassTypeId)
            {
                _logger.LogError($"Incompatible productId = {product.Id} with gympassTypeId = {gympassType.GympassTypeId}");
            }
        }

        public async Task EnsureProductCreated(GympassType gympassType)
        {
            var service = new ProductService();
            Product product;

            try
            {
                product = await service.GetAsync(gympassType.GympassTypeId);
            }
            catch (StripeException)
            {
                product = null;
            }

            if (product is null)
            {
                _logger.LogInformation($"Product with id {gympassType.GympassTypeId} does not exists");
                await CreateProduct(gympassType);
            }
        }

        public async Task CancelSubscription(string externalSubscriptionId)
        {
            var service = new SubscriptionService();
            try
            {
                await service.CancelAsync(externalSubscriptionId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Cannot cancel subscription with id = {externalSubscriptionId} on Stripe. Error: {ex.Message}");
            }
        }

        public async Task DeleteProduct(string gympassTypeId)
        {
            var service = new ProductService();

            var options = new ProductUpdateOptions
            {
                Active = false,
            };

            await service.UpdateAsync(gympassTypeId, options);
        }

        public async Task<string> CreateCheckoutSession(
            string gympassId,
            string customerId,
            string gympassTypeId,
            string successUrl,
            string cancelUrl)
        {
            var productService = new ProductService();
            var product = productService.Get(gympassTypeId);

            if (product is null || string.IsNullOrEmpty(product.DefaultPriceId))
            {
                _logger.LogError("Getting empty product or price when creating checkout session");
                throw new ArgumentException(nameof(product));
            }

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    Price = product.DefaultPriceId,
                    Quantity = 1,
                  },
                },
                Mode = PaymentModeType.subscription.ToString().ToLower(),
                SuccessUrl = UriHelper.AppendQueryParamToUri(successUrl, CarnetsConsts.GympassIdKey, gympassId),
                CancelUrl = UriHelper.AppendQueryParamToUri(cancelUrl, CarnetsConsts.GympassIdKey, gympassId),
                ClientReferenceId = customerId,
            };
            options.Metadata = new Dictionary<string, string>()
            {
                [CarnetsConsts.GympassIdKey] = gympassId
            };

            var sessionService = new SessionService();
            Session session = await sessionService.CreateAsync(options);

            return session.Url;
        }

        public PaymentResult HandlePaidResult(string jsonBody, string signature)
        {
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(jsonBody, signature, WebhookSecret);

                PaymentStatus paymentStatus = PaymentStatus.None;
                Func<Event, (string, string, string, string)> eventAdapter = null;

                // Handle the event
                if (stripeEvent.Type == Events.InvoicePaid)
                {
                    eventAdapter = GetDataFromInvoice;
                    paymentStatus = PaymentStatus.SubscriptionPaid;
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    eventAdapter = GetDataFromSubscription;
                    paymentStatus = PaymentStatus.SubscriptionDeleted;
                }
                else if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    eventAdapter = GetDataFromSession;
                    paymentStatus = PaymentStatus.SinglePaymentSuccess;
                }
                else if (stripeEvent.Type == Events.CheckoutSessionExpired)
                {
                    eventAdapter = GetDataFromSession;
                    paymentStatus = PaymentStatus.SinglePaymentExpired;
                }
                else
                {
                    _logger.LogInformation($"Unhandled Stripe event of type = {stripeEvent.Type}");
                    return PaymentResult.Empty();
                }

                var (gympassId, customerId, paymentMethodId, subscriptionId) = eventAdapter(stripeEvent);

                if (!string.IsNullOrEmpty(gympassId))
                {
                    return new PaymentResult(paymentStatus, gympassId, customerId, paymentMethodId, subscriptionId);
                }
                return ServeMissingPlanMetadata();
            }
            catch (StripeException e)
            {
                _logger.LogError($"Unhandled Stripe error = {e.Message}");
                throw new BadRequestException(e.StackTrace);
            }
        }

        private (string planId, string customerId, string paymentMethodId, string subscriptionId) GetDataFromSession(Event stripeEvent)
        {
            var session = stripeEvent.Data.Object as Session;
            string gympassId = string.Empty;

            var hasGympassIdMetadata = session.Metadata?.TryGetValue(CarnetsConsts.GympassIdKey, out gympassId) ?? false;
            if (hasGympassIdMetadata)
            {
                return (gympassId, session.CustomerId, string.Empty, session.SubscriptionId);
            }

            return (string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private (string planId, string customerId, string paymentMethodId, string subscriptionId) GetDataFromInvoice(Event stripeEvent)
        {
            var invoice = stripeEvent.Data.Object as Invoice;
            string gympassId = string.Empty;

            var hasGympassIdMetadata = invoice.Metadata?.TryGetValue(CarnetsConsts.GympassIdKey, out gympassId) ?? false;
            if (hasGympassIdMetadata)
            {
                return (gympassId, invoice.CustomerId, invoice.DefaultPaymentMethodId, invoice.SubscriptionId);
            }

            return (string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private (string planId, string customerId, string paymentMethodId, string subscriptionId) GetDataFromSubscription(Event stripeEvent)
        {
            var subscription = stripeEvent.Data.Object as Stripe.Subscription;
            string gympassId = string.Empty;

            var hasGympassIdMetadata = subscription.Metadata?.TryGetValue(CarnetsConsts.GympassIdKey, out gympassId) ?? false;
            if (hasGympassIdMetadata)
            {
                return (gympassId, subscription.CustomerId, subscription.DefaultPaymentMethodId, subscription.Id);
            }

            return (string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private PaymentResult ServeMissingPlanMetadata()
        {
            _logger.LogError($"Missing {CarnetsConsts.GympassIdKey} metadata");
            throw new ArgumentException($"Missing {CarnetsConsts.GympassIdKey} metadata");
        }
    }
}
