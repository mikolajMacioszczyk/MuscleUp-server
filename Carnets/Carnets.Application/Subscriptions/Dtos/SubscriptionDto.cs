using Carnets.Domain.Enums;

namespace Carnets.Application.Subscriptions.Dtos
{
    public class SubscriptionDto
    {
        public string SubscriptionId { get; set; }

        public string GympassId { get; set; }

        public GympassStatus GympassStatus { get; set; }

        public string StripeCustomerId { get; set; }

        public string StripePaymentmethodId { get; set; }
    }
}
