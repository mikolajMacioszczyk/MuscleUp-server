using Carnets.Application.Entries.Commands;
using Carnets.Application.Entries.Dtos;
using Carnets.Application.Interfaces;
using Carnets.Application.Models;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Exceptions;
using Common.Models;
using Moq;
using Xunit;

namespace CarnetsTests.EntriesTests
{
    public class CreateGymEntryTests
    {
        private Mock<IEntryTokenService> _entryTokenServiceMock;

        public CreateGymEntryTests()
        {
            _entryTokenServiceMock = new Mock<IEntryTokenService>();
            _entryTokenServiceMock.Setup(m => m.ValidateToken(It.IsAny<string>())).Returns(true);
        }

        [Fact]
        public async Task CreateGymEntry_ExpiredToken()
        {
            // arrange
            var entryToken = "Entry Token";
            var exirationDate = DateTime.UtcNow.AddMinutes(-1);
            var expectedError = "Entry token expired";

            var command = new CreateGymEntryCommand(new EntryTokenDto() { EntryToken = entryToken}, string.Empty);

            _entryTokenServiceMock.Setup(m => m.DecodeToken(entryToken)).Returns(new EntryTokenPayload()
            {
                ExpiresDate = exirationDate,
                GympassId = Guid.NewGuid().ToString()
            });

            var handler = new CreateGymEntryCommandHandler(
                    null, // logger
                    _entryTokenServiceMock.Object, // entryTokenService
                    null, // gympassRepository
                    null, // entryRepository
                    null // mapper
                );

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.ErrorCombined);
        }

        [Fact]
        public async Task CreateGymEntry_NotExisitingGympass()
        {
            // arrange
            var entryToken = "Entry Token";
            var exirationDate = DateTime.UtcNow.AddHours(1);
            var testGympassId = Guid.NewGuid().ToString();

            var command = new CreateGymEntryCommand(new EntryTokenDto() { EntryToken = entryToken }, string.Empty);

            _entryTokenServiceMock.Setup(m => m.DecodeToken(entryToken)).Returns(new EntryTokenPayload()
            {
                ExpiresDate = exirationDate,
                GympassId = testGympassId
            });

            var gympassRepositoryMock = new Mock<IGympassRepository>();
            gympassRepositoryMock.Setup(m => m.GetById(testGympassId, It.IsAny<bool>()))
                .ReturnsAsync(null as Gympass);

            var handler = new CreateGymEntryCommandHandler(
                    null, // logger
                    _entryTokenServiceMock.Object, // entryTokenService
                    gympassRepositoryMock.Object, // gympassRepository
                    null, // entryRepository
                    null // mapper
                );

            // assert
            await Assert.ThrowsAsync<BadRequestException>(
                async () => await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreateGymEntry_GympassNotFromFitnessClub()
        {
            // arrange
            var entryToken = "Entry Token";
            var exirationDate = DateTime.UtcNow.AddHours(1);
            var testGympassId = "e5d0115f-7ba2-423d-8cf9-959ae4e4cad5";
            var fitnessClubIdParam = "6dfbdddb-c856-4cf2-90c7-07e3addba596";
            var otherFitnessClubIdParam = "a36034f2-8c24-46e3-931f-6b7875852c35";
            var returnedGympass = new Gympass()
            {
                GympassId = testGympassId,
                GympassType = new GympassType()
                {
                    FitnessClubId = otherFitnessClubIdParam
                }
            };

            var command = new CreateGymEntryCommand(new EntryTokenDto() { EntryToken = entryToken }, fitnessClubIdParam);

            _entryTokenServiceMock.Setup(m => m.DecodeToken(entryToken)).Returns(new EntryTokenPayload()
            {
                ExpiresDate = exirationDate,
                GympassId = testGympassId
            });

            var gympassRepositoryMock = new Mock<IGympassRepository>();
            gympassRepositoryMock.Setup(m => m.GetById(testGympassId, It.IsAny<bool>()))
                .ReturnsAsync(returnedGympass);

            var handler = new CreateGymEntryCommandHandler(
                    null, // logger
                    _entryTokenServiceMock.Object, // entryTokenService
                    gympassRepositoryMock.Object, // gympassRepository
                    null, // entryRepository
                    null // mapper
                );

            // assert
            await Assert.ThrowsAsync<BadRequestException>(
                async () => await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreateGymEntry_NotEnoughtEntries()
        {
            // arrange
            var entryToken = "Entry Token";
            var exirationDate = DateTime.UtcNow.AddHours(1);
            var expectedError = "Remaining gympass entries cannot be less than 0";
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

            var command = new CreateGymEntryCommand(new EntryTokenDto() { EntryToken = entryToken }, fitnessClubIdParam);

            _entryTokenServiceMock.Setup(m => m.DecodeToken(entryToken)).Returns(new EntryTokenPayload()
            {
                ExpiresDate = exirationDate,
                GympassId = testGympassId
            });

            var gympassRepositoryMock = new Mock<IGympassRepository>();
            gympassRepositoryMock.Setup(m => m.GetById(testGympassId, It.IsAny<bool>()))
                .ReturnsAsync(returnedGympass);

            var handler = new CreateGymEntryCommandHandler(
                    null, // logger
                    _entryTokenServiceMock.Object, // entryTokenService
                    gympassRepositoryMock.Object, // gympassRepository
                    null, // entryRepository
                    null // mapper
                );

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.ErrorCombined);
        }

        [Fact]
        public async Task CreateGymEntry_InvalidToken()
        {
            // arrange
            var entryToken = "Invalid Entry Token";
            var exirationDate = DateTime.UtcNow.AddMinutes(-1);

            var command = new CreateGymEntryCommand(new EntryTokenDto() { EntryToken = entryToken }, string.Empty);

            _entryTokenServiceMock.Setup(m => m.ValidateToken(entryToken)).Returns(false);

            var handler = new CreateGymEntryCommandHandler(
                    null, // logger
                    _entryTokenServiceMock.Object, // entryTokenService
                    null, // gympassRepository
                    null, // entryRepository
                    null // mapper
                );

            // assert
            await Assert.ThrowsAsync<BadRequestException>(
                async () => await handler.Handle(command, CancellationToken.None));
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
            var expectedEntry = new Entry()
            {
                EntryId = "0ee21a77-9788-4b6e-9c73-a81c8f2fd215",
                CheckInTime = DateTime.UtcNow,
                Gympass = returnedGympass
            };

            var command = new CreateGymEntryCommand(new EntryTokenDto() { EntryToken = entryToken }, fitnessClubIdParam);

            _entryTokenServiceMock.Setup(m => m.DecodeToken(entryToken)).Returns(new EntryTokenPayload()
            {
                ExpiresDate = exirationDate,
                GympassId = testGympassId
            });

            var gympassRepositoryMock = new Mock<IGympassRepository>();
            gympassRepositoryMock.Setup(m => m.GetById(testGympassId, It.IsAny<bool>()))
                .ReturnsAsync(returnedGympass);
            gympassRepositoryMock.Setup(m => m.UpdateGympass(returnedGympass))
                .ReturnsAsync(new Result<Gympass>(returnedGympass));

            var entryRepository = new Mock<IEntryRepository>();
            entryRepository.Setup(m => m.CreateEntry(It.IsAny<Entry>()))
                .ReturnsAsync(expectedEntry);

            var handler = new CreateGymEntryCommandHandler(
                    null, // logger
                    _entryTokenServiceMock.Object, // entryTokenService
                    gympassRepositoryMock.Object, // gympassRepository
                    entryRepository.Object, // entryRepository
                    null // mapper
                );

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedEntry, result.Value);
            Assert.Equal(expectedRemainingEntries, returnedGympass.RemainingEntries);
        }
    }
}
