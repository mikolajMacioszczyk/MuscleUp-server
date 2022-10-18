using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Permissions.Queries
{
    public record GetPermissionByIdQuery : IRequest<PermissionBase>
    {
        public string PermissionId { get; init; }
        public string FitnessClubId { get; init; }
        public bool asTracking { get; init; }
    }

    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionBase>
    {
        private readonly IPermissionRepository<PerkPermission> _perkPermissionRepository;
        private readonly IPermissionRepository<ClassPermission> _classPermissionRepository;

        public GetPermissionByIdQueryHandler(
            IPermissionRepository<PerkPermission> perkPermissionRepository, 
            IPermissionRepository<ClassPermission> classPermissionRepository)
        {
            _perkPermissionRepository = perkPermissionRepository;
            _classPermissionRepository = classPermissionRepository;
        }

        public async Task<PermissionBase> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var perkPermissionFromDb = await _perkPermissionRepository
                .GetPermissionById(request.PermissionId, request.asTracking);

            var classPermissionFromDb = await _classPermissionRepository
                .GetPermissionById(request.PermissionId, request.asTracking);

            PermissionBase resultPermission = null;

            if (perkPermissionFromDb != null)
            {
                resultPermission = perkPermissionFromDb;
            }
            else if (classPermissionFromDb != null)
            {
                resultPermission = classPermissionFromDb;
            }

            if (resultPermission?.FitnessClubId != request.FitnessClubId)
            {
                resultPermission = null;
            }

            return resultPermission;
        }
    }
}
