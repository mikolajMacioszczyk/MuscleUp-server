using Carnets.Application.Enums;

namespace Carnets.Application.Models
{
    public class PaymentResult
    {
        public PaymentStatus PaymentStatus { get; set; }

        public string PlanId { get; set; }
    }
}
