using Carnets.Application.Enums;

namespace Carnets.Application.Models
{
    public class CheckoutSessionParams
    {
        public string GympassId { get; set; }

        public string CustomerId { get; set; }

        public string GympassTypeId { get; set; }

        public string SuccessUrl { get; set; }

        public string CancelUrl { get; set; }

        public string PriceId { get; set; }

        public PaymentModeType PaymentModeType { get; set; }

        public CheckoutSessionParams(
            string gympassId,
            string customerId,
            string gympassTypeId,
            string successUrl,
            string cancelUrl,
            string priceId,
            PaymentModeType paymentModeType)
        {
            GympassId = gympassId;
            CustomerId = customerId;
            GympassTypeId = gympassTypeId;
            SuccessUrl = successUrl;
            CancelUrl = cancelUrl;
            PriceId = priceId;
            PaymentModeType = paymentModeType;
        }
    }
}
