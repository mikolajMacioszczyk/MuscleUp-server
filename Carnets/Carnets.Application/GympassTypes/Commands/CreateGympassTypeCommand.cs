using AutoMapper;
using Carnets.Application.Helpers;
using Carnets.Application.Interfaces;
using Carnets.Application.Permissions.Queries;
using Carnets.Domain.Models;
using Common.Exceptions;
using MediatR;

namespace Carnets.Application.GympassTypes.Commands
{
    public record CreateGympassTypeCommand : IRequest<GympassTypeWithPermissions>
    {
        public GympassType GympassType { get; set; }
        public IEnumerable<string> ClassPermissionsNames { get; set; }
        public IEnumerable<string> PerkPermissionsNames { get; set; }
    }

    public class CreateGympassTypeCommandHandler : IRequestHandler<CreateGympassTypeCommand, GympassTypeWithPermissions>
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IPaymentService _paymentService;
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public CreateGympassTypeCommandHandler(
            IGympassTypeRepository gympassTypeRepository,
            IPaymentService paymentService,
            IAssignedPermissionRepository assignedPermissionRepository,
            IMapper mapper,
            ISender mediator)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _paymentService = paymentService;
            _assignedPermissionRepository = assignedPermissionRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<GympassTypeWithPermissions> Handle(CreateGympassTypeCommand request, CancellationToken cancellationToken)
        {
            var (classPermissions, perkPermissions) = await GetAllPermissions(request);

            var createResult = await _gympassTypeRepository.CreateGympassType(request.GympassType);

            if (!createResult.IsSuccess)
            {
                throw new BadRequestException(createResult.ErrorCombined);
            }

            await AssignAllGympassPermissions(classPermissions, perkPermissions, createResult.Value);

            await _paymentService.CreateProduct(createResult.Value);

            await _gympassTypeRepository.SaveChangesAsync();
            await _assignedPermissionRepository.SaveChangesAsync();

            return GympassTypeHelper.MapToGympassTypeWithPermissions(createResult.Value,
                request.ClassPermissionsNames, request.PerkPermissionsNames, _mapper);
        }

        private async Task<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)> GetAllPermissions(CreateGympassTypeCommand request)
        {
            var getPermissionsQuery = new GetPermissionsByNamesQuery()
            {
                ClassPermissionsNames = request.ClassPermissionsNames,
                PerkPermissionsNames = request.PerkPermissionsNames
            };

            var allPermissionResult = await _mediator.Send(getPermissionsQuery);
            if (!allPermissionResult.IsSuccess)
            {
                throw new BadRequestException(allPermissionResult.ErrorCombined);
            }
            return allPermissionResult.Value;
        }

        private async Task AssignAllGympassPermissions(
            IEnumerable<ClassPermission> classPermissions, 
            IEnumerable<PerkPermission> perkPermissions,
            GympassType gympassType)
        {
            var assignResult = await GympassTypeHelper.AssignAllGympassPermissions(classPermissions,
                perkPermissions, gympassType, _mediator);

            if (!assignResult.IsSuccess)
            {
                throw new BadRequestException(assignResult.ErrorCombined);
            }
        }
    }
}
