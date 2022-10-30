using Carnets.Application.Consts;
using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Application.Entries.Helpers
{
    public static class EntryHelper
    {
        private static int MinutesInHour = 60;

        public static (bool result, string reason) CanGympassEnterGym(Gympass gympass)
        {
            // inactive gympass
            if (gympass.Status != GympassStatus.Active)
            {
                return (false, CarnetsConsts.GympassNotActive);
            }

            // not valid date
            if (gympass.ValidityDate < DateTime.UtcNow)
            {
                return (false, CarnetsConsts.GympassValidityEnded);
            }

            // entries already used
            if (gympass.GympassType.ValidationType == GympassTypeValidation.Entries
                && gympass.RemainingEntries <= 0)
            {
                return (false, CarnetsConsts.GympassNoMoreEntries);
            }

            // validate entry minute
            var now = DateTime.Now;
            var currentMinute = now.Hour * MinutesInHour + now.Minute;

            if (currentMinute < gympass.GympassType.EnableEntryFromInMinutes
                || currentMinute > gympass.GympassType.EnableEntryToInMinutes)
            {
                return (false, CarnetsConsts.GympassEntryMinuteNotAllowed);
            }

            return (true, string.Empty);
        }
        public static async Task<Result<Gympass>> ReduceGympassEntries(Gympass gympass, IGympassRepository gympassRepository)
        {
            if (gympass is null) throw new ArgumentException(nameof(gympass));

            if (gympass.GympassType.ValidationType != GympassTypeValidation.Entries)
            {
                return new Result<Gympass>(gympass);
            }
            gympass.RemainingEntries = gympass.RemainingEntries - 1;

            if (gympass.RemainingEntries < 0)
            {
                return new Result<Gympass>($"Remaining gympass entries cannot be less than 0");
            }
            var updateResult = await gympassRepository.UpdateGympass(gympass);

            return updateResult;
        }
    }
}
