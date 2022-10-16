using Carnets.Domain.Enums;

namespace Carnets.Domain.Models
{
    public class PaymentResult
    {
        public PaymentStatus PaymentStatus { get; set; }

        public string PlanId { get; set; }
    }
}
