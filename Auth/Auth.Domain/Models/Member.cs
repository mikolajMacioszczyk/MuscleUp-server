using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Models
{
    public class Member : SpecificUserBase
    {
        [Range(0, double.MaxValue)]
        public double HeightInCm { get; set; }

        [Range(0, double.MaxValue)]
        public double WeightInKg { get; set; }
    }
}
