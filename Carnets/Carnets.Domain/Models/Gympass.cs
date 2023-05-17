using Carnets.Domain.Enums;
using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class Gympass
    {
        [Key]
        [MaxLength(36)]
        public string GympassId { get; set; }

        [Required]
        [MaxLength(36)]
        public string UserId { get; set; }

        [Required]
        public GympassType GympassType { get; set; }

        public DateTime ValidityDate { get; set; }

        public DateTime ActivationDate { get; set; }

        public GympassStatus Status { get; set; }

        [Range(0, int.MaxValue)]
        public int RemainingEntries { get; set; }

        public PaymentType PaymentType { get; set; }

        public Result<bool> CanGympassEnterGym()
        {
            // inactive gympass
            if (Status != GympassStatus.Active)
            {
                return new Result<bool>("Gympass not active");
            }

            // not valid date
            if (ValidityDate < DateTime.UtcNow)
            {
                return new Result<bool>("Gympass validity ended");
            }

            // entries already used
            if (GympassType.ValidationType == GympassTypeValidation.Entries
                && RemainingEntries <= 0)
            {
                return new Result<bool>("Gympass has not enough entries");
            }

            // validate entry minute
            var now = DateTime.UtcNow;
            var minutesInHour = 60;
            var currentMinute = now.Hour * minutesInHour + now.Minute;

            if (currentMinute < GympassType.EnableEntryFromInMinutes
                || currentMinute > GympassType.EnableEntryToInMinutes)
            {
                return new Result<bool>("Gympass entry minute not allowed");
            }

            return new Result<bool>(true);
        }

        public Result<int> ReduceEntries()
        {
            if (GympassType.ValidationType != GympassTypeValidation.Entries)
            {
                return new Result<int>(RemainingEntries < 0 ? 0 : RemainingEntries);
            }

            if (RemainingEntries <= 0)
            {
                return new Result<int>($"Remaining gympass entries cannot be less than 0");
            }

            RemainingEntries = RemainingEntries - 1;

            return new Result<int>(RemainingEntries);
        }
    }
}
