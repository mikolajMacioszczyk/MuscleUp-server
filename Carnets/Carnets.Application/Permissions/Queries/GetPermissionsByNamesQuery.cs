using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.Permissions.Queries
{
    public record GetPermissionsByNamesQuery : IRequest<Result<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)>>
    {
        public IEnumerable<string> ClassPermissionsNames { get; set; }
        public IEnumerable<string> PerkPermissionsNames { get; set; }
    }

    public class GetPermissionsByNamesQueryHandler : IRequestHandler<GetPermissionsByNamesQuery, Result<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)>>
    {
        private readonly IPermissionRepository<ClassPermission> _classPermissionRepository;
        private readonly IPermissionRepository<PerkPermission> _perkPermissionRepository;

        public GetPermissionsByNamesQueryHandler(
            IPermissionRepository<ClassPermission> classPermissionRepository, 
            IPermissionRepository<PerkPermission> perkPermissionRepository)
        {
            _classPermissionRepository = classPermissionRepository;
            _perkPermissionRepository = perkPermissionRepository;
        }

        public async Task<Result<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)>> Handle(GetPermissionsByNamesQuery request, CancellationToken cancellationToken)
        {
            // get all classPermissions
            var classPermissions = await _classPermissionRepository
                .GetAllPermissionsByNames(request.ClassPermissionsNames ?? Array.Empty<string>(), true);

            if (!classPermissions.IsSuccess)
            {
                return new Result<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)>(classPermissions.Errors);
            }

            // get all perkPemrissions
            var perkPermissions = await _perkPermissionRepository
                .GetAllPermissionsByNames(request.PerkPermissionsNames ?? Array.Empty<string>(), true);

            if (!perkPermissions.IsSuccess)
            {
                return new Result<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)>(perkPermissions.Errors);
            }

            return new Result<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)>((classPermissions.Value, perkPermissions.Value));

        }
    }
}
