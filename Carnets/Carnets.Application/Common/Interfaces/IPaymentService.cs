using Carnets.Application.Models;
using Carnets.Domain.Models;

namespace Carnets.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<string> GetOrCreateCustomer(string userId);

        Task EnsureProductCreated(GympassType gympassTypeId);

        Task CreateProduct(GympassType gympassType);

        PaymentResult HandlePaidResult(string jsonBody, string signature);

        Task<string> CreateCheckoutSession(CheckoutSessionParams checkoutSessionParams);

        Task CancelSubscription(string externalSubscriptionId);

        Task DeleteProduct(string gympassTypeId);
    }
}
