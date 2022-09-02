using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class GympassTypeRepository : IGympassTypeRepository
    {
        private readonly CarnetsDbContext _context;

        public GympassTypeRepository(CarnetsDbContext context)
        {
            _context = context;
        }

        public async Task<GympassType?> GetGympassById(string gympassId)
        {
            if (string.IsNullOrWhiteSpace(gympassId))
            {
                return null;
            }

            return await _context.GympassTypes.FirstOrDefaultAsync(g => g.GympassTypeId == gympassId);
        }

        public async Task<IEnumerable<GympassType>> GetAllActiveGympassTypes()
        {
            return await _context.GympassTypes.Where(g => g.IsActive).ToListAsync();
        }

        private async Task<GympassType?> GetActiveGympassTypeByNameAndFitnessClub(string name, string fitnessClubId)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(fitnessClubId))
            {
                return null;
            }

            return await _context.GympassTypes
                .Where(g => g.GympassTypeName == name && g.FitnessClubId == fitnessClubId)
                .FirstOrDefaultAsync(g => g.IsActive);
        }

        public async Task<Result<GympassType>> CreateGympassType(GympassType gympassType)
        {
            var isUnique = (
                await GetActiveGympassTypeByNameAndFitnessClub(gympassType.GympassTypeName, gympassType.FitnessClubId)
                ) is null;
            
            if (!isUnique)
            {
                return new Result<GympassType>($"GympassType with name {gympassType.GympassTypeName} already exists");
            }

            gympassType.GympassTypeId = Guid.NewGuid().ToString();
            gympassType.IsActive = true;
            gympassType.Version = 1;

            await _context.GympassTypes.AddAsync(gympassType);
            await _context.SaveChangesAsync();

            return new Result<GympassType>(gympassType);
        }

        public async Task<Result<GympassType>> UpdateGympassType(GympassType gympassType)
        {
            var gympassFromDb = await GetGympassById(gympassType.GympassTypeId);
            if (gympassFromDb is null)
            {
                return new Result<GympassType>(Common.CommonConsts.NOT_FOUND);
            }

            if (!gympassType.IsActive)
            {
                return new Result<GympassType>("Operation not permitted. An inactive gympass type cannot be changed.");
            }

            var updated = new GympassType()
            {
                GympassTypeId = Guid.NewGuid().ToString(),
                GympassTypeName = gympassFromDb.GympassTypeName,
                FitnessClubId = gympassFromDb.FitnessClubId,
                IsActive = true,
                Version = gympassFromDb.Version + 1
            };

            // set previous version inactive
            gympassType.IsActive = false;

            // update domain properties
            updated.Price = gympassType.Price;
            updated.ValidityPeriodInSeconds = gympassType.ValidityPeriodInSeconds;

            await _context.GympassTypes.AddAsync(updated);

            // todo: link permission
            var linkedAssignedPermissions = await GetAllGympassTypeAssignedPermissions(gympassType.GympassTypeId);
            foreach (var oldAssignedPermission in linkedAssignedPermissions)
            {
                var newAssignedPermission = new AssignedPermission
                {
                    Permission = oldAssignedPermission.Permission,
                    PermissionId = oldAssignedPermission.PermissionId,
                    GympassType = updated,
                    GympassTypeId = updated.GympassTypeId
                };

                await _context.AssignedPermissions.AddAsync(newAssignedPermission);
            }

            await _context.SaveChangesAsync();

            return new Result<GympassType>(updated);
        }

        public async Task<Result<bool>> DeleteGympassType(string gympassTypeId)
        {
            var gympassFromDb = await GetGympassById(gympassTypeId);
            if (gympassFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            var linkedGympasses = await GetAllGympassesWithType(gympassTypeId);

            if (linkedGympasses.Any())
            {
                return new Result<bool>("Operation not permitted. Cannot delete GympassType with linked Gympasses");
            }

            var linkedAssignedPermissions = await GetAllGympassTypeAssignedPermissions(gympassTypeId);

            foreach (var assignedPermission in linkedAssignedPermissions)
            {
                _context.AssignedPermissions.Remove(assignedPermission);
            }

            _context.GympassTypes.Remove(gympassFromDb);
            await _context.SaveChangesAsync();

            return new Result<bool>(true);
        }

        private async Task<IEnumerable<Gympass>> GetAllGympassesWithType(string gympassTypeId)
        {
            return await _context.Gympasses.Include(g => g.GympassType)
                .Where(g => g.GympassType.GympassTypeId == gympassTypeId)
                .ToListAsync();
        }

        private async Task<IEnumerable<AssignedPermission>> GetAllGympassTypeAssignedPermissions(string gympassTypeId)
        {
            return await _context.AssignedPermissions
                .Include(p => p.GympassType)
                .Include(p => p.Permission)
                .Where(p => p.GympassTypeId == gympassTypeId)
                .ToListAsync();
        }
    }
}
