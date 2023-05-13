using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Xunit;

namespace CarnetsTests.UnitTests.DomianTests.GympassTests
{
    public class ReduceEntriesTests
    {
        [Theory]
        [InlineData(10, 9)]
        [InlineData(int.MaxValue, int.MaxValue - 1)]
        [InlineData(1, 0)]
        public void ReduceEntries_Valid(int initialGympassEntries, int expectedEntries)
        {
            // arrange
            var gympass = new Gympass()
            {
                RemainingEntries = initialGympassEntries,
                GympassType = GetGympassTypeWithEntriesValidation()
            };

            // act
            var reduceEntriesResult = gympass.ReduceEntries();

            // assert
            Assert.NotNull(reduceEntriesResult);
            Assert.True(reduceEntriesResult.IsSuccess);
            Assert.Equal(reduceEntriesResult.Value, expectedEntries);
            Assert.Empty(reduceEntriesResult.ErrorCombined);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        public void ReduceEntries_NotEnoughtEntries(int initialGympassEntries)
        {
            // arrange
            var expectedReason = "Remaining gympass entries cannot be less than 0";
            var gympass = new Gympass()
            {
                RemainingEntries = initialGympassEntries,
                GympassType = GetGympassTypeWithEntriesValidation()
            };

            // act
            var reduceEntriesResult = gympass.ReduceEntries();

            // assert
            Assert.NotNull(reduceEntriesResult);
            Assert.False(reduceEntriesResult.IsSuccess);
            Assert.Equal(expectedReason, reduceEntriesResult.ErrorCombined);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(int.MaxValue, int.MaxValue)]
        [InlineData(1, 1)]
        [InlineData(0, 0)]
        [InlineData(int.MinValue, 0)]
        [InlineData(-1, 0)]
        public void ReduceEntries_TimeValidationMethod(int initialGympassEntries, int expectedEntries)
        {
            // arrange
            var gympass = new Gympass()
            {
                RemainingEntries = initialGympassEntries,
                GympassType = GetGympassTypeWithTimeValidation()
            };

            // act
            var reduceEntriesResult = gympass.ReduceEntries();

            // assert
            Assert.NotNull(reduceEntriesResult);
            Assert.True(reduceEntriesResult.IsSuccess);
            Assert.Equal(reduceEntriesResult.Value, expectedEntries);
            Assert.Empty(reduceEntriesResult.ErrorCombined);
        }

        private GympassType GetGympassTypeWithEntriesValidation() => new GympassType()
            {
                ValidationType = GympassTypeValidation.Entries
            };

        private GympassType GetGympassTypeWithTimeValidation() => new GympassType()
        {
            ValidationType = GympassTypeValidation.Time
        };
    }
}
