using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Carnets.Repo.Repositories;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarnetsTests.IntegrationTests.RepositoriesTests.EntryRepositoryTests
{
    public class EntryRepositoryTests : RepoTestClassBase
    {
        private readonly IEntryRepository _entryRepository;
        private readonly EntryData _entryData = new EntryData();

        public EntryRepositoryTests() : base($"{nameof(EntryRepositoryTests)}_{Guid.NewGuid()}")
        {
            _entryRepository = new EntryRepository(_context);
            SeedDefaultEntries();
        }

        [Fact]
        public async Task GetByIdTests_Match()
        {
            // arrange
            var searchId = EntryData.Entry1Id;

            // act
            var entryResult = await _entryRepository.GetEntryById(searchId, false);

            // assert
            Assert.NotNull(entryResult);
            Assert.Equal(entryResult.EntryId, searchId);
            Assert.NotNull(entryResult.Gympass);
        }

        [Fact]
        public async Task GetByIdTests_NotMatch()
        {
            // arrange
            var searchId = Guid.NewGuid().ToString();

            // act
            var entryResult = await _entryRepository.GetEntryById(searchId, false);

            // assert
            Assert.Null(entryResult);
        }

        [Fact]
        public async Task SearchEntries_All()
        {
            // arrange
            var expectedEntries = _entryData.DefaultEntries;

            // act
            var resultEntries = await _entryRepository.SearchEntries(_ => true, 0, int.MaxValue, false);

            // assert
            Assert.NotNull(resultEntries);
            Assert.Equal(expectedEntries.AsJson(), resultEntries.AsJson());
        }

        [Fact]
        public async Task SearchEntries_All_EmptyCollection()
        {
            // arrange
            _context.RemoveRange(await _context.Entries.ToListAsync());
            await _context.SaveChangesAsync();

            // act
            var resultEntries = await _entryRepository.SearchEntries(_ => true, 0, int.MaxValue, false);

            // assert
            Assert.NotNull(resultEntries);
            Assert.False(resultEntries.Any());
        }

        [Fact]
        public async Task SearchEntries_ByGympass()
        {
            // arrange
            var expectedEntries = _entryData.DefaultEntries.Where(e => e.Gympass == _entryData.DefaultGympass1);

            // act
            var resultEntries = await _entryRepository.SearchEntries(e => e.Gympass == _entryData.DefaultGympass1, 0, int.MaxValue, false);

            // assert
            Assert.NotNull(resultEntries);
            Assert.Equal(expectedEntries.AsJson(), resultEntries.AsJson());
        }

        [Fact]
        public async Task SearchEntries_ByProperty()
        {
            // arrange
            var expectedEntries = _entryData.DefaultEntries.Where(e => e.Entered);

            // act
            var resultEntries = await _entryRepository.SearchEntries(e => e.Entered, 0, int.MaxValue, false);

            // assert
            Assert.NotNull(resultEntries);
            Assert.Equal(expectedEntries.OrderBy(o => o.EntryId).AsJson(), resultEntries.OrderBy(o => o.EntryId).AsJson());
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(0, 2, 2)]
        [InlineData(0, 3, 3)]
        [InlineData(1, 1, 1)]
        [InlineData(1, 2, 1)]
        [InlineData(1, 3, 0)]
        [InlineData(2, 1, 1)]
        [InlineData(2, 2, 0)]
        [InlineData(2, 3, 0)]

        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(-1, -1, 0)]
        public async Task SearchEntries_Pagged(int pageNumber, int pageSize, int expectedPageSize)
        {
            // arrange
            var expectedEntries = _entryData.DefaultEntries.Where(e => e.Entered);

            // act
            var resultEntries = await _entryRepository.SearchEntries(_ => true, pageNumber, pageSize, false);

            // assert
            Assert.NotNull(resultEntries);
            Assert.True(resultEntries.All(e => e != null));
            Assert.Equal(expectedPageSize, resultEntries.Count());
        }

        [Fact]
        public async Task CreateEntry_Valid()
        {
            // arrange
            const string initialEntryId = "ced8d17f-f704-4e63-8892-f2a6e4b0142f";
            var entry = new Entry()
            {
                EntryId = initialEntryId,
                Gympass = _entryData.DefaultGympass1,
            };

            // act
            var entryResult = await _entryRepository.CreateEntry(entry);
            await _entryRepository.SaveChangesAsync();

            var entryFromGet = await _entryRepository.GetEntryById(entryResult.EntryId, false);

            // assert
            Assert.NotNull(entryResult);
            Assert.NotEqual(entryResult.EntryId, initialEntryId);
            Assert.NotNull(entryFromGet);
        }

        [Fact]
        public async Task CreateEntry_Null()
        {
            // arrange
            Entry entry = null;

            // assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _entryRepository.CreateEntry(entry));
        }

        [Fact]
        public async Task UpdateEntry_Successful()
        {
            // arrange
            var entryToUpdateId = EntryData.Entry1Id;
            var checkInTime = DateTime.UtcNow.AddDays(10);
            var checkOutTime = DateTime.UtcNow.AddDays(11);
            var entryExpirationTime = DateTime.UtcNow.AddDays(12);

            Entry updateEntry = new()
            {
                Entered = false,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime,
                EntryExpirationTime = entryExpirationTime,
            };
            Entry expectedModel = new()
            {
                EntryId = entryToUpdateId,
                Entered = false,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime,
                EntryExpirationTime = entryExpirationTime,
                Gympass = _entryData.DefaultGympass1
            };

            // act
            Result<Entry> updateResult = await _entryRepository.UpdateEntry(entryToUpdateId, updateEntry);
            await _entryRepository.SaveChangesAsync();

            // assert
            Assert.NotNull(updateResult);
            Assert.True(updateResult.IsSuccess);
            Assert.NotNull(updateResult.Value);
            Assert.Equal(expectedModel.AsJson(), updateResult.Value.AsJson());
        }

        [Fact]
        public async Task UpdateEntry_NotChangingIdAndGympassData()
        {
            // arrange
            var entryToUpdateId = EntryData.Entry1Id;
            var checkInTime = DateTime.UtcNow.AddDays(10);
            var checkOutTime = DateTime.UtcNow.AddDays(11);
            var entryExpirationTime = DateTime.UtcNow.AddDays(12);

            Entry updateEntry = new()
            {
                EntryId = "Updated",
                Entered = false,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime,
                EntryExpirationTime = entryExpirationTime,
                Gympass = _entryData.DefaultGympass2
            };
            Entry expectedModel = new()
            {
                EntryId = entryToUpdateId,
                Entered = false,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime,
                EntryExpirationTime = entryExpirationTime,
                Gympass = _entryData.DefaultGympass1
            };

            // act
            Result<Entry> updateResult = await _entryRepository.UpdateEntry(entryToUpdateId, updateEntry);
            await _entryRepository.SaveChangesAsync();

            // assert
            Assert.NotNull(updateResult);
            Assert.True(updateResult.IsSuccess);
            Assert.NotNull(updateResult.Value);
            Assert.Equal(expectedModel.AsJson(), updateResult.Value.AsJson());
        }

        [Fact]
        public async Task UpdateEntry_NotFound()
        {
            // arrange
            const string entryToUpdateId = "Not existing";
            var checkInTime = DateTime.UtcNow.AddDays(10);
            var checkOutTime = DateTime.UtcNow.AddDays(11);
            var entryExpirationTime = DateTime.UtcNow.AddDays(12);
            string error = Common.CommonConsts.NOT_FOUND;

            Entry updateEntry = new()
            {
                Entered = false,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime,
                EntryExpirationTime = entryExpirationTime,
            };

            // act
            Result<Entry> updateResult = await _entryRepository.UpdateEntry(entryToUpdateId, updateEntry);
            await _entryRepository.SaveChangesAsync();

            // assert
            Assert.NotNull(updateResult);
            Assert.False(updateResult.IsSuccess);
            Assert.Null(updateResult.Value);
            Assert.Equal(error, updateResult.ErrorCombined);
        }

        [Fact]
        public async Task UpdateEntry_NullId()
        {
            // arrange
            string entryToUpdateId = null;
            var checkInTime = DateTime.UtcNow.AddDays(10);
            var checkOutTime = DateTime.UtcNow.AddDays(11);
            var entryExpirationTime = DateTime.UtcNow.AddDays(12);
            string error = Common.CommonConsts.NOT_FOUND;

            Entry updateEntry = new()
            {
                Entered = false,
                CheckInTime = checkInTime,
                CheckOutTime = checkOutTime,
                EntryExpirationTime = entryExpirationTime,
            };

            // act
            Result<Entry> updateResult = await _entryRepository.UpdateEntry(entryToUpdateId, updateEntry);
            await _entryRepository.SaveChangesAsync();

            // assert
            Assert.NotNull(updateResult);
            Assert.False(updateResult.IsSuccess);
            Assert.Null(updateResult.Value);
            Assert.Equal(error, updateResult.ErrorCombined);
        }

        [Fact]
        public async Task UpdateEntry_NullEntry()
        {
            // arrange
            var entryToUpdateId = EntryData.Entry1Id;
            Entry updateEntry = null;

            // assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _entryRepository.UpdateEntry(entryToUpdateId, updateEntry));
        }

        private void SeedDefaultEntries()
        {
            _context.GympassTypes.Add(_entryData.DefaultGympassType);

            _context.Gympasses.Add(_entryData.DefaultGympass1);
            _context.Gympasses.Add(_entryData.DefaultGympass2);

            _context.Entries.AddRange(_entryData.DefaultEntries);
            _context.SaveChanges();
        }
    }
}
