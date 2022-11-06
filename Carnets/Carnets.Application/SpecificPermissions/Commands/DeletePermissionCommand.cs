using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace Carnets.Application.SpecificPermissions.Commands
{
    public record DeletePermissionCommand<TPermission> : IRequest<Result<bool>>
        where TPermission : PermissionBase
    {
        public string PermissionId { get; init; }
        public string FitnessClubId { get; init; }
    }

    public class DeletePermissionCommandHandler<TPermission> : IRequestHandler<DeletePermissionCommand<TPermission>, Result<bool>>
        where TPermission : PermissionBase
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IPermissionRepository<TPermission> _permissionRepository;
        private readonly ILogger<DeletePermissionCommandHandler<TPermission>> _logger;
        private readonly IQueueService _queueService;

        public DeletePermissionCommandHandler(
            IPermissionRepository<TPermission> permissionRepository,
            IAssignedPermissionRepository assignedPermissionRepository,
            ILogger<DeletePermissionCommandHandler<TPermission>> logger,
            IQueueService queueService)
        {
            _permissionRepository = permissionRepository;
            _assignedPermissionRepository = assignedPermissionRepository;
            _queueService = queueService;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeletePermissionCommand<TPermission> request, CancellationToken cancellationToken)
        {
            var permission = await _permissionRepository.GetPermissionById(request.PermissionId, false);

            var result = await _permissionRepository.DeletePermission(request.PermissionId, request.FitnessClubId);

            if (result.IsSuccess)
            {
                var connectedPermissions = await _assignedPermissionRepository
                    .GetAllByPermission(request.PermissionId, true);

                foreach (var assignedPermission in connectedPermissions)
                {
                    var removeAssignedPermissionResult = await _assignedPermissionRepository
                        .RemovePermission(assignedPermission.PermissionId, assignedPermission.GympassTypeId, request.FitnessClubId);

                    if (!removeAssignedPermissionResult.IsSuccess)
                    {
                        return removeAssignedPermissionResult;
                    }
                }

                await _assignedPermissionRepository.SaveChangesAsync();
                await _permissionRepository.SaveChangesAsync();

                await NotifyPermissionDeleted(permission, request.PermissionId);
            }

            return result;
        }

        private async Task NotifyPermissionDeleted(TPermission permission, string permissionId)
        {
            if (permission != null)
            {
                await _queueService.SendAsync(
                @object: permission,
                exchangeName: Common.CommonConsts.DeletedPermissionExchangeName,
                routingKey: Common.CommonConsts.DeletedPermissionQueueName
                );
            }
            else
            {
                _logger.LogError($"Deleted permission with id = {permission} is null when brodcasting message");
            }
        }
    }
}
