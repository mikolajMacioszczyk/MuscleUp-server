using Carnets.Application.Interfaces;
using Carnets.Application.Permissions.Queries;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.AssignedPermissions.Commands
{
    public record RemovePermissionWithAllAssigementsCommand : IRequest<Result<bool>>
    {
        public string PermissionId { get; init; }
        public string FitnessClubId { get; init; }
    }

    public class RemovePermissionWithAllAssigementsCommandHandler : IRequestHandler<RemovePermissionWithAllAssigementsCommand, Result<bool>>
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IPermissionRepository<ClassPermission> _classPermissionRepository;
        private readonly IPermissionRepository<PerkPermission> _perkPermissionRepository;
        private readonly ISender _mediator;

        public RemovePermissionWithAllAssigementsCommandHandler(
            IAssignedPermissionRepository assignedPermissionRepository,
            IPermissionRepository<ClassPermission> classPermissionRepository,
            IPermissionRepository<PerkPermission> perkPermissionRepository,
            ISender mediator)
        {
            _assignedPermissionRepository = assignedPermissionRepository;
            _classPermissionRepository = classPermissionRepository;
            _perkPermissionRepository = perkPermissionRepository;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(RemovePermissionWithAllAssigementsCommand request, CancellationToken cancellationToken)
        {
            var permissionFromDb = await _mediator.Send(new GetPermissionByIdQuery()
            {
                PermissionId = request.PermissionId,
                FitnessClubId = request.FitnessClubId,
                asTracking = true
            });

            if (permissionFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            var assignedPermissions = await _assignedPermissionRepository
                .GetAllByPermission(request.PermissionId, true);

            foreach (var assignedPermission in assignedPermissions)
            {
                await _assignedPermissionRepository
                    .RemovePermission(request.PermissionId, assignedPermission.GympassTypeId, request.FitnessClubId);
            }

            switch (permissionFromDb.PermissionType)
            {
                case PermissionType.PerkPermission:
                    await _perkPermissionRepository.DeletePermission(request.PermissionId, request.FitnessClubId);
                    await _perkPermissionRepository.SaveChangesAsync();
                    break;
                case PermissionType.ClassPermission:
                    await _classPermissionRepository.DeletePermission(request.PermissionId, request.FitnessClubId);
                    await _classPermissionRepository.SaveChangesAsync();
                    break;
            }

            // TODO: Does exists pattern to commit transactions?
            await _assignedPermissionRepository.SaveChangesAsync();

            return new Result<bool>(true);
        }
    }
}
