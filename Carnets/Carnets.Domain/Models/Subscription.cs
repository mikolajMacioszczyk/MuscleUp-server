using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class Subscription
    {
        [Key]
        [MaxLength(30)]
        public string SubscriptionId { get; set; }

        [Required]
        public Gympass Gympass { get; set; }

        [Required]
        [MaxLength(30)]
        // TODO: Unique
        public string StripeCustomerId { get; set; }

        [Required]
        [MaxLength(30)]
        public string StripePaymentmethodId { get; set; }
    }
}
