using System.ComponentModel.DataAnnotations;

namespace Carnets.Application.Subscriptions.Dtos
{
    public class CreateGympassSubscriptionDto
    {
        [Required]
        [MaxLength(36)]
        public string GympassId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethodId { get; set; }
    }
}
