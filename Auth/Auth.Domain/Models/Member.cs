using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Models
{
    public class Member : SpecificUserBase
    {
        [Range(0, double.MaxValue)]
        public double Height { get; set; }

        [Range(0, double.MaxValue)]
        public double Weight { get; set; }
    }
}
