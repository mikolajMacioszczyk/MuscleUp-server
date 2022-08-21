using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string StripeCustomerId { get; set; }

        [Required]
        [MaxLength(30)]
        public string StripePaymentmethodId { get; set; }
    }
}
