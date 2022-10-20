﻿using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Application.Interfaces
{
    public interface IGympassTypeRepository
    {
        Task<GympassType> GetGympassTypeById(string gympassId, bool asTracking);

        Task<IEnumerable<GympassType>> GetAllGympassTypes(string fitnessClubId, bool onlyActive,
            int pageNumber, int pageSize, bool asTracking);

        Task<Result<GympassType>> CreateGympassType(GympassType gympassType);

        Task<Result<GympassType>> UpdateGympassType(GympassType gympassType);

        Task<Result<bool>> DeleteGympassType(string gympassTypeId);

        Task SaveChangesAsync();
    }
}