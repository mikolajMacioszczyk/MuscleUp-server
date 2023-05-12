using Carnets.Domain.Enums;
using Carnets.Domain.Models;

namespace CarnetsTests.IntegrationTests.RepositoriesTests.EntryRepositoryTests
{
    public class EntryData
    {
        internal static readonly string Entry1Id = "238f32fc-4332-4356-9b68-450c7050d863";
        internal static readonly string Entry2Id = "0c4757fa-a74a-41cb-b4e3-523329765e3a";
        internal static readonly string Entry3Id = "dd112397-a0a5-4abd-a5fe-df2b20226e60";
        internal static readonly string Gympass1Id = "e3f7b2b5-72f7-4c68-8b53-d01d539d9e88";
        internal static readonly string Gympass2Id = "d45597d7-b411-46b4-84cc-8f3a9c2993af";

        internal GympassType DefaultGympassType { get; private set; }

        internal Gympass DefaultGympass1 { get; private set; }

        internal Gympass DefaultGympass2 { get; private set; }

        internal List<Entry> DefaultEntries { get; private set; }

        public EntryData()
        {
            DefaultGympassType = new GympassType()
            {
                GympassTypeId = Guid.NewGuid().ToString(),
                FitnessClubId = Guid.NewGuid().ToString(),
                AllowedEntries = 12,
                Description = "",
                EnableEntryFromInMinutes = int.MinValue,
                EnableEntryToInMinutes = int.MaxValue,
                GympassTypeName = Guid.NewGuid().ToString(),
                Interval = IntervalType.Day,
                IntervalCount = 100,
                IsActive = true,
                OneTimePriceId = Guid.NewGuid().ToString(),
                ReccuringPriceId = Guid.NewGuid().ToString(),
                Price = 1,
                ValidationType = GympassTypeValidation.Time,
                Version = 1
            };

            DefaultGympass1 = new Gympass()
            {
                GympassId = Gympass1Id,
                GympassType = DefaultGympassType,
                UserId = Guid.NewGuid().ToString(),
                ActivationDate = DateTime.Now.AddDays(-2),
                PaymentType = PaymentType.OneTime,
                RemainingEntries = 10,
                Status = GympassStatus.Active,
                ValidityDate = DateTime.Now.AddDays(1),
            };

            DefaultGympass2 = new Gympass()
            {
                GympassId = Gympass2Id,
                GympassType = DefaultGympassType,
                UserId = Guid.NewGuid().ToString(),
                ActivationDate = DateTime.Now.AddDays(-1),
                PaymentType = PaymentType.Recurring,
                RemainingEntries = 12,
                Status = GympassStatus.Inactive,
                ValidityDate = DateTime.Now.AddDays(-1),
            };

            DefaultEntries = new List<Entry>
            {
                new Entry(){ EntryId = Entry1Id, Gympass = DefaultGympass1, CheckInTime = DateTime.Now.AddMinutes(-5), Entered = true, EntryExpirationTime = DateTime.Now.AddDays(1) },
                new Entry(){ EntryId = Entry2Id, Gympass = DefaultGympass1, Entered = false, EntryExpirationTime = DateTime.Now.AddDays(1) },
                new Entry(){ EntryId = Entry3Id, Gympass = DefaultGympass1, CheckInTime = DateTime.Now.AddMinutes(-50), CheckOutTime = DateTime.Now.AddMinutes(-10), Entered = true, EntryExpirationTime = DateTime.Now.AddDays(1) }
            };
        }
    }
}
