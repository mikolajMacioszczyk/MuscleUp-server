using Carnets.Repo;
using Microsoft.EntityFrameworkCore;

namespace CarnetsTests.IntegrationTests.RepositoriesTests
{
    public abstract class RepoTestClassBase : IDisposable
    {
        protected readonly CarnetsDbContext _context;

        public RepoTestClassBase(string dbName)
        {
            var options = new DbContextOptionsBuilder<CarnetsDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

            _context = new CarnetsDbContext(options);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
