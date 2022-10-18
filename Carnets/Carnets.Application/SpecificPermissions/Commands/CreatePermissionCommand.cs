using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.SpecificPermissions.Commands
{
    public record CreatePermissionCommand<TPermission> : IRequest<Result<TPermission>>
        where TPermission : PermissionBase
    {
        public TPermission NewPermission { get; init; }
    }

    public class CreatePermissionCommandHandler<TPermission> : IRequestHandler<CreatePermissionCommand<TPermission>, Result<TPermission>>
        where TPermission : PermissionBase
    {
        private readonly IPermissionRepository<TPermission> _permissionRepository;

        public CreatePermissionCommandHandler(IPermissionRepository<TPermission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<Result<TPermission>> Handle(CreatePermissionCommand<TPermission> request, CancellationToken cancellationToken)
        {
            var result = await _permissionRepository.CreatePermission(request.NewPermission);

            if (result.IsSuccess)
            {
                await _permissionRepository.SaveChangesAsync();
            }

            return result;
        }
    }
}
