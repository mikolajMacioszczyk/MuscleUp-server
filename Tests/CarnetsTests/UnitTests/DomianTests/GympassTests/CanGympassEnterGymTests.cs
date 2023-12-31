﻿using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using Xunit;

namespace CarnetsTests.UnitTests.DomianTests.GympassTests
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
            const string expectedReason = "Gympass not active";

            // act
            var canEnterResult = gympass.CanGympassEnterGym();

            // assert
            AssertFailedWithReason(canEnterResult, expectedReason);
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
            const string expectedReason = "Gympass validity ended";

            // act
            var canEnterResult = gympass.CanGympassEnterGym();

            // assert
            AssertFailedWithReason(canEnterResult, expectedReason);
        }

        [Fact]
        public void CanGympassEnterGym_NotEnoughEntries()
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
            const string expectedReason = "Gympass has not enough entries";

            // act
            var canEnterResult = gympass.CanGympassEnterGym();

            // assert
            AssertFailedWithReason(canEnterResult, expectedReason);
        }

        [Fact]
        public void CanGympassEnterGym_BeforeEntryMinute()
        {
            // arrange
            var now = DateTime.UtcNow;
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
            const string expectedReason = "Gympass entry minute not allowed";

            // act
            var canEnterResult = gympass.CanGympassEnterGym();

            // assert
            AssertFailedWithReason(canEnterResult, expectedReason);
        }

        [Fact]
        public void CanGympassEnterGym_AfterEntryMinute()
        {
            // arrange
            var now = DateTime.UtcNow;
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
            const string expectedReason = "Gympass entry minute not allowed";

            // act
            var canEnterResult = gympass.CanGympassEnterGym();

            // assert
            AssertFailedWithReason(canEnterResult, expectedReason);
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
            var canEnterResult = gympass.CanGympassEnterGym();

            // assert
            Assert.True(canEnterResult.IsSuccess);
            Assert.True(canEnterResult.Value);
            Assert.Empty(canEnterResult.ErrorCombined);
        }

        private void AssertFailedWithReason(Result<bool> result, string expectedReason)
        {
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedReason, result.ErrorCombined);
        }
    }
}
