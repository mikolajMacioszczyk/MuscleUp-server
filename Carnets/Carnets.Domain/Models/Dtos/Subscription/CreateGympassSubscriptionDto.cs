using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models.Dtos
{
    public class CreateGympassSubscriptionDto
    {
        [Required]
        [MaxLength(36)]
        public string GympassId { get; set; }

        [Required]
        [MaxLength(30)]
        public string StripeCustomerId { get; set; }

        [Required]
        [MaxLength(30)]
        public string StripePaymentmethodId { get; set; }
    }
}
