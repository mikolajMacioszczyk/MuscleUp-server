using Carnets.Application.Enums;

namespace Carnets.Application.Models
{
    public class PaymentResult
    {
        public PaymentStatus PaymentStatus { get; set; }

        public string PlanId { get; set; }

        public string CustomerId { get; set; }

        public string PaymentMethodId { get; set; }

        public string ExternalSubscriptionId { get; set; }

        public PaymentResult(
            PaymentStatus paymentStatus, 
            string planId, 
            string customerId, 
            string paymentMethodId, 
            string externalSubscriptionId)
        {
            PaymentStatus = paymentStatus;
            PlanId = planId;
            CustomerId = customerId;
            PaymentMethodId = paymentMethodId;
            ExternalSubscriptionId = externalSubscriptionId;
        }

        public static PaymentResult Empty() => 
            new PaymentResult(PaymentStatus.None, string.Empty, string.Empty, string.Empty, string.Empty);
    }
}
