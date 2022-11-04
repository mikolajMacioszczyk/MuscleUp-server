using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class Subscription
    {
        [Key]
        [MaxLength(36)]
        public string SubscriptionId { get; set; }

        [Required]
        public Gympass Gympass { get; set; }

        [Required]
        [MaxLength(50)]
        public string StripeCustomerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ExternalSubscriptionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string StripePaymentmethodId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastPaymentDate { get; set; }

        public bool IsActive { get; set; }
    }
}
