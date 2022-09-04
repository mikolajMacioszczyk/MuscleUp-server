﻿using Carnets.Domain.Interfaces;
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
            var allowedEntriesPermissionFromDb = _context.AllowedEntriesPermissions
                .FirstOrDefaultAsync(p => p.PermissionId == grantRequest.PermissionId);

            var classPermissionFromDb = _context.ClassPermissions
                .FirstOrDefaultAsync(p => p.PermissionId == grantRequest.PermissionId);

            var timePermissionFromDb = _context.TimePermissionEntries
                .FirstOrDefaultAsync(p => p.PermissionId == grantRequest.PermissionId);

            var gympassTypeFromDb = _context.GympassTypes
                .FirstOrDefaultAsync(g => g.GympassTypeId == grantRequest.GympassTypeId);

            var alreadyGrantedFromDb = _context.AssignedPermissions
                .FirstOrDefaultAsync(a => a.GympassTypeId == grantRequest.GympassTypeId && a.PermissionId == grantRequest.PermissionId);

            var allDbRequests = new Task[] { allowedEntriesPermissionFromDb, classPermissionFromDb, timePermissionFromDb, gympassTypeFromDb, alreadyGrantedFromDb };

            await Task.WhenAll(allDbRequests);

            PermissionBase permission = null;

            if (allowedEntriesPermissionFromDb.Result != null)
            {
                permission = allowedEntriesPermissionFromDb.Result;
            }
            else if (classPermissionFromDb.Result != null)
            {
                permission = classPermissionFromDb.Result;
            }
            else if (timePermissionFromDb.Result != null)
            {
                permission = timePermissionFromDb.Result;
            }

            if (permission is null || gympassTypeFromDb.Result is null)
            {
                return new Result<AssignedPermission>(Common.CommonConsts.NOT_FOUND);
            }

            // is unique
            if (alreadyGrantedFromDb.Result != null)
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
    }
}
