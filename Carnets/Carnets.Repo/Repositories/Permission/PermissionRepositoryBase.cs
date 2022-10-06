using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Exceptions;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public abstract class PermissionRepositoryBase<TPermission> : IPermissionRepository<TPermission>
        where TPermission : PermissionBase
    {
        protected readonly CarnetsDbContext _context;

        protected PermissionRepositoryBase(CarnetsDbContext? context)
        {
            _context = context;
        }

        protected abstract DbSet<TPermission> PermissionDbSet { get; }

        public async Task<IEnumerable<TPermission>> GetAll(string fitnessClubId, bool asTracking)
        {
            var query = PermissionDbSet
                .Where(p => p.FitnessClubId == fitnessClubId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<TPermission> GetPermissionById(string permissionId, bool asTracking)
        {
            IQueryable<TPermission> query = PermissionDbSet;

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(p => p.PermissionId == permissionId);
        }

        public virtual async Task<Result<TPermission>> CreatePermission(TPermission newPermission)
        {
            newPermission.PermissionId = Guid.NewGuid().ToString();

            await PermissionDbSet.AddAsync(newPermission);
            await _context.SaveChangesAsync();

            return new Result<TPermission>(newPermission);
        }

        public async Task<Result<bool>> DeletePermission(string permissionId, string fitnessClubId)
        {
            var permissionFromDb = await GetPermissionById(permissionId, true);

            if (permissionFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            if (permissionFromDb.FitnessClubId != fitnessClubId)
            {
                throw new BadRequestException("Cannot manage permission");
            }

            PermissionDbSet.Remove(permissionFromDb);
            
            return new Result<bool>(true);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
