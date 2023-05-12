using Carnets.Application.Entries.Commands;
using Carnets.Application.Entries.Dtos;
using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Exceptions;
using Common.Models;
using Moq;
using Xunit;

namespace CarnetsTests.UnitTests.EntriesTests
{
    public class CreateGymEntryTests
    {
        private readonly Mock<IGympassRepository> gympassRepositoryMock = new();
        private readonly Mock<IEntryRepository> entryRepositoryMock = new();

        [Fact]
        public async Task CreateGymEntry_ExpiredToken()
        {
            // arrange
            var entryToken = "Entry Token";
            var expirationDate = DateTime.UtcNow.AddMinutes(-1);
            var expectedError = "Entry token expired";

            var command = new EnterGymCommand(new EntryTokenDto() { EntryToken = entryToken }, string.Empty);

            entryRepositoryMock.Setup(m => m.GetEntryById(entryToken, It.IsAny<bool>()))
                .ReturnsAsync(new Entry()
                {
                    EntryExpirationTime = expirationDate,
                });

            var handler = new EnterGymCommandHandler(
                    null, // logger
                    gympassRepositoryMock.Object, // gympassRepository
                    entryRepositoryMock.Object // entryRepository
                );

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.ErrorCombined);
        }

        [Fact]
        public async Task CreateGymEntry_GympassNotFromFitnessClub()
        {
            // arrange
            var entryToken = "Entry Token";
            var expirationDate = DateTime.UtcNow.AddHours(1);
            var testGympassId = "e5d0115f-7ba2-423d-8cf9-959ae4e4cad5";
            var fitnessClubIdParam = "6dfbdddb-c856-4cf2-90c7-07e3addba596";
            var otherFitnessClubIdParam = "a36034f2-8c24-46e3-931f-6b7875852c35";
            var expectedError = "Gympass does not belongs to fitness club";
            var returnedGympass = new Gympass()
            {
                GympassId = testGympassId,
                GympassType = new GympassType()
                {
                    FitnessClubId = otherFitnessClubIdParam
                }
            };

            var command = new EnterGymCommand(new EntryTokenDto() { EntryToken = entryToken }, fitnessClubIdParam);

            gympassRepositoryMock.Setup(m => m.GetById(testGympassId, It.IsAny<bool>()))
                .ReturnsAsync(returnedGympass);

            entryRepositoryMock.Setup(m => m.GetEntryById(entryToken, It.IsAny<bool>()))
                .ReturnsAsync(new Entry()
                {
                    EntryExpirationTime = expirationDate,
                    Gympass = returnedGympass
                });

            var handler = new EnterGymCommandHandler(
                    null, // logger
                    gympassRepositoryMock.Object, // gympassRepository
                    entryRepositoryMock.Object // entryRepository
                );

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.ErrorCombined);
        }

        [Fact]
        public async Task CreateGymEntry_NotEnoughtEntries()
        {
            // arrange
            var entryToken = "Entry Token";
            var expirationDate = DateTime.UtcNow.AddHours(1);
            var testGympassId = "e5d0115f-7ba2-423d-8cf9-959ae4e4cad5";
            var fitnessClubIdParam = "6dfbdddb-c856-4cf2-90c7-07e3addba596";
            var returnedGympass = new Gympass()
            {
                GympassId = testGympassId,
                GympassType = new GympassType()
                {
                    FitnessClubId = fitnessClubIdParam,
                    ValidationType = GympassTypeValidation.Entries
                },
                Status = GympassStatus.Active,
                RemainingEntries = 0
            };

            var command = new EnterGymCommand(new EntryTokenDto() { EntryToken = entryToken }, fitnessClubIdParam);

            gympassRepositoryMock.Setup(m => m.GetById(testGympassId, It.IsAny<bool>()))
                .ReturnsAsync(returnedGympass);

            entryRepositoryMock.Setup(m => m.GetEntryById(entryToken, It.IsAny<bool>()))
                .ReturnsAsync(new Entry()
                {
                    EntryExpirationTime = expirationDate,
                    Gympass = returnedGympass
                });

            var handler = new EnterGymCommandHandler(
                    null, // logger
                    gympassRepositoryMock.Object, // gympassRepository
                    entryRepositoryMock.Object // entryRepository
                );

            // assert
            await Assert.ThrowsAsync<BadRequestException>(
                async () => await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreateGymEntry_InvalidToken()
        {
            // arrange
            var entryToken = "Invalid Entry Token";
            var exirationDate = DateTime.UtcNow.AddMinutes(-1);
            var expectedError = "Invalid entry token";

            var command = new EnterGymCommand(new EntryTokenDto() { EntryToken = entryToken }, string.Empty);

            entryRepositoryMock.Setup(m => m.GetEntryById(entryToken, It.IsAny<bool>()))
                .ReturnsAsync(null as Entry);

            var handler = new EnterGymCommandHandler(
                    null, // logger
                    null, // gympassRepository
                    entryRepositoryMock.Object // entryRepository
                );

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.ErrorCombined);
        }

        [Theory]
        [InlineData(GympassTypeValidation.Time, 1, 1)]
        [InlineData(GympassTypeValidation.Time, 0, 0)]
        [InlineData(GympassTypeValidation.Entries, 1, 0)]
        public async Task CreateGymEntry_Valid(GympassTypeValidation gympassValidationType,
            int remainingEntries, int expectedRemainingEntries)
        {
            // arrange
            var entryToken = "Entry Token";
            var exirationDate = DateTime.UtcNow.AddHours(1);
            var testGympassId = "e5d0115f-7ba2-423d-8cf9-959ae4e4cad5";
            var fitnessClubIdParam = "6dfbdddb-c856-4cf2-90c7-07e3addba596";
            var returnedGympass = new Gympass()
            {
                GympassId = testGympassId,
                GympassType = new GympassType()
                {
                    FitnessClubId = fitnessClubIdParam,
                    ValidationType = gympassValidationType
                },
                Status = GympassStatus.Active,
                RemainingEntries = remainingEntries
            };

            var getEntry = new Entry()
            {
                EntryId = entryToken,
                CheckInTime = DateTime.UtcNow,
                Gympass = returnedGympass,
                Entered = false,
                EntryExpirationTime = exirationDate
            };

            var command = new EnterGymCommand(new EntryTokenDto() { EntryToken = entryToken }, fitnessClubIdParam);

            gympassRepositoryMock.Setup(m => m.GetById(testGympassId, It.IsAny<bool>()))
                .ReturnsAsync(returnedGympass);
            gympassRepositoryMock.Setup(m => m.UpdateGympass(returnedGympass))
                .ReturnsAsync(new Result<Gympass>(returnedGympass));

            entryRepositoryMock.Setup(m => m.GetEntryById(entryToken, It.IsAny<bool>()))
                .ReturnsAsync(getEntry);

            entryRepositoryMock.Setup(m => m.UpdateEntry(entryToken, getEntry))
                .ReturnsAsync(new Result<Entry>(getEntry));

            var handler = new EnterGymCommandHandler(
                    null, // logger
                    gympassRepositoryMock.Object, // gympassRepository
                    entryRepositoryMock.Object // entryRepository
                );

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(entryToken, result.Value.EntryId);
            Assert.True(result.Value.Entered);
            Assert.Equal(expectedRemainingEntries, returnedGympass.RemainingEntries);
        }
    }
}
