using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
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

        public async Task<Result<IEnumerable<AssignedPermission>>> GetAllGympassPermissions(string gympassTypeId)
        {
            var gympass = await _context.GympassTypes.FirstOrDefaultAsync(g => g.GympassTypeId == gympassTypeId);
            if (gympass is null)
            {
                return new Result<IEnumerable<AssignedPermission>>(Common.CommonConsts.NOT_FOUND);
            }

            var assignedPermission = await _context.AssignedPermissions
                .Include(p => p.Permission)
                .Include(p => p.GympassType)
                .Where(p => p.GympassTypeId == gympassTypeId)
                .ToListAsync();

            return new Result<IEnumerable<AssignedPermission>>(assignedPermission);
        }

        // Permission may be granted also to inactive gympass types
        public async Task<Result<AssignedPermission>> GrantPermission(AssignedPermission grantRequest)
        {
            var gympassTypeFromDb = await _context.GympassTypes
                .FirstOrDefaultAsync(g => g.GympassTypeId == grantRequest.GympassTypeId);

            var permission = await GetPermissionById(grantRequest.PermissionId);

            if (permission is null || gympassTypeFromDb is null)
            {
                return new Result<AssignedPermission>(Common.CommonConsts.NOT_FOUND);
            }

            var alreadyGrantedFromDb = await _context.AssignedPermissions
                .FirstOrDefaultAsync(a => a.GympassTypeId == grantRequest.GympassTypeId && a.PermissionId == grantRequest.PermissionId);

            // is unique
            if (alreadyGrantedFromDb != null)
            {
                return new Result<AssignedPermission>("Operation not permitted. Permission has beend already assigned to GympassType");
            }

            await _context.AssignedPermissions.AddAsync(grantRequest);
            await _context.SaveChangesAsync();
            
            // ok
            return new Result<AssignedPermission>(grantRequest);
        }

        public async Task<Result<bool>> RevokePermission(string permissionId, string gympassTypeId)
        {
            var assignedPermissionFromDb = await _context.AssignedPermissions
                .FirstOrDefaultAsync(p => p.PermissionId == permissionId && p.GympassTypeId == gympassTypeId);

            if (assignedPermissionFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            _context.AssignedPermissions.Remove(assignedPermissionFromDb);
            await _context.SaveChangesAsync();
            return new Result<bool>(true);
        }

        public async Task<Result<bool>> RemovePermissionWithAllAssigements(string permissionId)
        {
            var permissionFromDb = await GetPermissionById(permissionId);

            if (permissionFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            var assignedPermissions = await _context.AssignedPermissions
                .Where(p => p.PermissionId == permissionId).ToListAsync();

            foreach (var assignedPermission in assignedPermissions)
            {
                _context.AssignedPermissions.Remove(assignedPermission);
            }

            switch (permissionFromDb.PermissionType)
            {
                case Domain.Enums.PermissionType.TimePermissionEntry:
                    _context.TimePermissionEntries.Remove(permissionFromDb as TimePermissionEntry);
                    break;
                case Domain.Enums.PermissionType.AllowedEntriesPermission:
                    _context.AllowedEntriesPermissions.Remove(permissionFromDb as AllowedEntriesPermission);
                    break;
                case Domain.Enums.PermissionType.ClassPermission:
                    _context.ClassPermissions.Remove(permissionFromDb as ClassPermission);
                    break;
            }

            await _context.SaveChangesAsync();
            return new Result<bool>(true);
        }

        private async Task<PermissionBase> GetPermissionById(string permissionId)
        {
            var allowedEntriesPermissionFromDb = await _context.AllowedEntriesPermissions
                .FirstOrDefaultAsync(p => p.PermissionId == permissionId);

            var classPermissionFromDb = await _context.ClassPermissions
                .FirstOrDefaultAsync(p => p.PermissionId == permissionId);

            var timePermissionFromDb = await _context.TimePermissionEntries
                .FirstOrDefaultAsync(p => p.PermissionId == permissionId);

            if (allowedEntriesPermissionFromDb != null)
            {
                return allowedEntriesPermissionFromDb;
            }
            else if (classPermissionFromDb != null)
            {
                return classPermissionFromDb;
            }
            else if (timePermissionFromDb != null)
            {
                return timePermissionFromDb;
            }

            return null;
        }
    }
}
