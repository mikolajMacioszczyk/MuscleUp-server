using Carnets.Application.Entries.Commands;
using Carnets.Application.Entries.Dtos;
using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Exceptions;
using Common.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CarnetsTests.UnitTests.ApplicationTests.EntriesTests
{
    public class CreateGymEntryTests
    {
        private readonly Mock<IGympassRepository> _gympassRepositoryMock = new();
        private readonly Mock<IEntryRepository> _entryRepositoryMock = new();
        private readonly Mock<ILogger<EnterGymCommandHandler>> _logger = new();

        [Fact]
        public async Task CreateGymEntry_ExpiredToken()
        {
            // arrange
            const string entryToken = "Entry Token";
            var expirationDate = DateTime.UtcNow.AddMinutes(-1);
            const string expectedError = "Entry token expired";

            var command = new EnterGymCommand(new EntryTokenDto() { EntryToken = entryToken }, string.Empty);

            _entryRepositoryMock.Setup(m => m.GetEntryById(entryToken, It.IsAny<bool>()))
                .ReturnsAsync(new Entry()
                {
                    EntryExpirationTime = expirationDate,
                });

            var handler = CreateHandlerWithMocks();

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
            const string entryToken = "Entry Token";
            var expirationDate = DateTime.UtcNow.AddHours(1);
            const string testGympassId = "e5d0115f-7ba2-423d-8cf9-959ae4e4cad5";
            const string fitnessClubIdParam = "6dfbdddb-c856-4cf2-90c7-07e3addba596";
            const string otherFitnessClubIdParam = "a36034f2-8c24-46e3-931f-6b7875852c35";
            const string expectedError = "Gympass does not belongs to fitness club";
            var returnedGympass = new Gympass()
            {
                GympassId = testGympassId,
                GympassType = new GympassType()
                {
                    FitnessClubId = otherFitnessClubIdParam
                }
            };

            var command = new EnterGymCommand(new EntryTokenDto() { EntryToken = entryToken }, fitnessClubIdParam);

            _gympassRepositoryMock.Setup(m => m.GetById(testGympassId, It.IsAny<bool>()))
                .ReturnsAsync(returnedGympass);

            _entryRepositoryMock.Setup(m => m.GetEntryById(entryToken, It.IsAny<bool>()))
                .ReturnsAsync(new Entry()
                {
                    EntryExpirationTime = expirationDate,
                    Gympass = returnedGympass
                });

            var handler = CreateHandlerWithMocks();

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
            const string entryToken = "Entry Token";
            var expirationDate = DateTime.UtcNow.AddHours(1);
            const string testGympassId = "e5d0115f-7ba2-423d-8cf9-959ae4e4cad5";
            const string fitnessClubIdParam = "6dfbdddb-c856-4cf2-90c7-07e3addba596";
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

            _gympassRepositoryMock.Setup(m => m.GetById(testGympassId, It.IsAny<bool>()))
                .ReturnsAsync(returnedGympass);

            _entryRepositoryMock.Setup(m => m.GetEntryById(entryToken, It.IsAny<bool>()))
                .ReturnsAsync(new Entry()
                {
                    EntryExpirationTime = expirationDate,
                    Gympass = returnedGympass
                });

            var handler = CreateHandlerWithMocks();

            // assert
            await Assert.ThrowsAsync<InvalidInputException>(
                async () => await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreateGymEntry_InvalidToken()
        {
            // arrange
            const string entryToken = "Invalid Entry Token";
            var exirationDate = DateTime.UtcNow.AddMinutes(-1);
            const string expectedError = "Invalid entry token";

            var command = new EnterGymCommand(new EntryTokenDto() { EntryToken = entryToken }, string.Empty);

            _entryRepositoryMock.Setup(m => m.GetEntryById(entryToken, It.IsAny<bool>()))
                .ReturnsAsync(null as Entry);

            var handler = CreateHandlerWithMocks();

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
            const string entryToken = "Entry Token";
            var exirationDate = DateTime.UtcNow.AddHours(1);
            const string testGympassId = "e5d0115f-7ba2-423d-8cf9-959ae4e4cad5";
            const string fitnessClubIdParam = "6dfbdddb-c856-4cf2-90c7-07e3addba596";
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

            _gympassRepositoryMock.Setup(m => m.GetById(testGympassId, It.IsAny<bool>()))
                .ReturnsAsync(returnedGympass);
            _gympassRepositoryMock.Setup(m => m.UpdateGympass(returnedGympass))
                .ReturnsAsync(new Result<Gympass>(returnedGympass));

            _entryRepositoryMock.Setup(m => m.GetEntryById(entryToken, It.IsAny<bool>()))
                .ReturnsAsync(getEntry);

            _entryRepositoryMock.Setup(m => m.UpdateEntry(entryToken, getEntry))
                .ReturnsAsync(new Result<Entry>(getEntry));

            var handler = CreateHandlerWithMocks();

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(entryToken, result.Value.EntryId);
            Assert.True(result.Value.Entered);
            Assert.Equal(expectedRemainingEntries, returnedGympass.RemainingEntries);
        }

        private EnterGymCommandHandler CreateHandlerWithMocks() =>
            new EnterGymCommandHandler(
                _logger.Object,
                _gympassRepositoryMock.Object,
                _entryRepositoryMock.Object
            );
    }
}
