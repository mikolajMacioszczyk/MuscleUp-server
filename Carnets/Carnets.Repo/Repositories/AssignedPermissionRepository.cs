using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Exceptions;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class AssignedPermissionRepository : IAssignedPermissionRepository
    {
        private readonly CarnetsDbContext _context;

        public AssignedPermissionRepository(CarnetsDbContext context)
        {
            _context = context;
        }

        public async Task<AssignedPermission> GetAssignedPermissionById(string gympassTypeId, string permissionId, bool asTracking)
        {
            IQueryable<AssignedPermission> query = _context.AssignedPermissions
                .Include(a => a.Permission)
                .Include(a => a.GympassType);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(a => a.GympassTypeId == gympassTypeId && a.PermissionId == permissionId);
        }

        public async Task<IEnumerable<AssignedPermission>> GetAllByPermission(string permissionId, bool asTracking)
        {
            IQueryable<AssignedPermission> query = _context.AssignedPermissions
                .Where(a => a.PermissionId == permissionId)
                .Include(a => a.Permission)
                .Include(a => a.GympassType);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<Result<IEnumerable<AssignedPermission>>> GetAllGympassPermissions(string gympassTypeId, bool asTracking)
        {
            var assignedPermissionQuery = _context.AssignedPermissions
                .Include(p => p.GympassType)
                .Include(p => p.Permission)
                .Where(p => p.GympassTypeId == gympassTypeId);

            if (!asTracking)
            {
                assignedPermissionQuery = assignedPermissionQuery.AsNoTracking();
            }

            return new Result<IEnumerable<AssignedPermission>>(await assignedPermissionQuery.ToListAsync());
        }

        public async Task<Result<AssignedPermission>> CreateAssignedPermission(AssignedPermission assignedPermission)
        {
            var alreadyGrantedFromDb = await GetAssignedPermissionById(assignedPermission.GympassTypeId, assignedPermission.PermissionId, false);

            // is unique
            if (alreadyGrantedFromDb != null)
            {
                return new Result<AssignedPermission>("Operation not permitted. Permission has beend already assigned to GympassType");
            }

            await _context.AssignedPermissions.AddAsync(assignedPermission);

            return new Result<AssignedPermission>(assignedPermission);
        }

        public async Task<Result<bool>> RemovePermission(string permissionId, string gympassTypeId, string fitnessClubId)
        {
            var assignedPermissionFromDb = await GetAssignedPermissionById(gympassTypeId, permissionId, true);

            if (assignedPermissionFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            if (assignedPermissionFromDb.Permission.FitnessClubId != fitnessClubId)
            {
                throw new BadRequestException("Cannot manage Permission from other FitnessClub");
            }
            else if (assignedPermissionFromDb.GympassType.FitnessClubId != fitnessClubId)
            {
                throw new BadRequestException("Cannot manage GympassType from other FitnessClub");
            }

            _context.AssignedPermissions.Remove(assignedPermissionFromDb);
            
            return new Result<bool>(true);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
