using Carnets.Application.Consts;
using Carnets.Application.Entries.Helpers;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Xunit;

namespace CarnetsTests.EntriesTests
{
    public class CanGympassEnterGymTests
    {
        private const int MinutesInHour = 60;

        [Theory]
        [InlineData(GympassStatus.New)]
        [InlineData(GympassStatus.Inactive)]
        [InlineData(GympassStatus.Cancelled)]
        [InlineData(GympassStatus.Completed)]
        public void CanGympassEnterGym_NotActiveGympass(GympassStatus gympassStatus)
        {
            // arrange
            var gympass = new Gympass()
            {
                Status = gympassStatus
            };
            var expectedReason = CarnetsConsts.GympassNotActive;

            // act
            var (predicateResult, reason) = EntryHelper.CanGympassEnterGym(gympass);

            // assert
            Assert.False(predicateResult);
            Assert.Equal(expectedReason, reason);
        }


        [Theory]
        [InlineData(GympassTypeValidation.Time)]
        [InlineData(GympassTypeValidation.Entries)]
        public void CanGympassEnterGym_ExtendedValidityDate(GympassTypeValidation validationType)
        {
            // arrange
            var gympass = new Gympass()
            {
                Status = GympassStatus.Active,
                ValidityDate = DateTime.UtcNow.AddDays(-1),
                RemainingEntries = 1,
                GympassType = new GympassType()
                {
                    ValidationType = validationType
                }
            };
            var expectedReason = CarnetsConsts.GympassValidityEnded;

            // act
            var (predicateResult, reason) = EntryHelper.CanGympassEnterGym(gympass);

            // assert
            Assert.False(predicateResult);
            Assert.Equal(expectedReason, reason);
        }

        [Fact]
        public void CanGympassEnterGym_NotEnoughtEntries()
        {
            // arrange
            var gympass = new Gympass()
            {
                Status = GympassStatus.Active,
                ValidityDate = DateTime.UtcNow.AddDays(1),
                RemainingEntries = 0,
                GympassType = new GympassType()
                {
                    ValidationType = GympassTypeValidation.Entries
                }
            };
            var expectedReason = CarnetsConsts.GympassNoMoreEntries;

            // act
            var (predicateResult, reason) = EntryHelper.CanGympassEnterGym(gympass);

            // assert
            Assert.False(predicateResult);
            Assert.Equal(expectedReason, reason);
        }

        [Fact]
        public void CanGympassEnterGym_BeforeEntryMinute()
        {
            // arrange
            var now = DateTime.Now;
            var bottomMinute = now.Hour * MinutesInHour + now.Minute + 1;

            var gympass = new Gympass()
            {
                Status = GympassStatus.Active,
                ValidityDate = DateTime.UtcNow.AddDays(1),
                RemainingEntries = 1,
                GympassType = new GympassType()
                {
                    ValidationType = GympassTypeValidation.Time,
                    EnableEntryFromInMinutes = bottomMinute,
                    EnableEntryToInMinutes = int.MaxValue,
                }
            };
            var expectedReason = CarnetsConsts.GympassEntryMinuteNotAllowed;

            // act
            var (predicateResult, reason) = EntryHelper.CanGympassEnterGym(gympass);

            // assert
            Assert.False(predicateResult);
            Assert.Equal(expectedReason, reason);
        }

        [Fact]
        public void CanGympassEnterGym_AfterEntryMinute()
        {
            // arrange
            var now = DateTime.Now;
            var closingMinute = now.Hour * MinutesInHour + now.Minute - 1;

            var gympass = new Gympass()
            {
                Status = GympassStatus.Active,
                ValidityDate = DateTime.UtcNow.AddDays(1),
                RemainingEntries = 1,
                GympassType = new GympassType()
                {
                    ValidationType = GympassTypeValidation.Time,
                    EnableEntryFromInMinutes = 0,
                    EnableEntryToInMinutes = closingMinute,
                }
            };
            var expectedReason = CarnetsConsts.GympassEntryMinuteNotAllowed;

            // act
            var (predicateResult, reason) = EntryHelper.CanGympassEnterGym(gympass);

            // assert
            Assert.False(predicateResult);
            Assert.Equal(expectedReason, reason);
        }

        [Theory]
        [InlineData(GympassTypeValidation.Time, 0)]
        [InlineData(GympassTypeValidation.Time, 1)]
        [InlineData(GympassTypeValidation.Entries, 1)]
        public void CanGympassEnterGym_Valid(GympassTypeValidation validationType, int remainingEntries)
        {
            // arrange
            var gympass = new Gympass()
            {
                Status = GympassStatus.Active,
                ValidityDate = DateTime.UtcNow.AddDays(1),
                RemainingEntries = remainingEntries,
                GympassType = new GympassType()
                {
                    ValidationType = validationType,
                    EnableEntryFromInMinutes = 0,
                    EnableEntryToInMinutes = int.MaxValue,
                }
            };

            // act
            var (predicateResult, reason) = EntryHelper.CanGympassEnterGym(gympass);

            // assert
            Assert.True(predicateResult);
            Assert.Empty(reason);
        }
    }
}
