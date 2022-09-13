using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    // TODO: WorkerId from Token
    public abstract class SpecificPermissionControllerBase<TPermission, TPermissionDto, TCreatePermissionDto> : ControllerBase
        where TPermission : PermissionBase
    {
        private readonly IPermissionRepository<TPermission> _permissionRepository;
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly IMapper _mapper;

        protected SpecificPermissionControllerBase(
            IPermissionRepository<TPermission> permissionRepository, 
            IMapper mapper, 
            IFitnessClubHttpService fitnessClubHttpService)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _fitnessClubHttpService = fitnessClubHttpService;
        }

        [HttpGet("{workerId}")]
        public async Task<ActionResult<IEnumerable<TPermissionDto>>> GetAllPermisions([FromRoute] string workerId)
        {
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);
            var fitnessClubId = fitnessClubResult.Value.FitnessClubId;

            return Ok(_mapper.Map<IEnumerable<TPermissionDto>>(await _permissionRepository.GetAll(fitnessClubId)));
        }

        [HttpGet("{permissionId}/{workerId}")]
        public async Task<ActionResult> GetPermission([FromRoute] string permissionId, [FromRoute] string workerId)
        {
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var permission = await _permissionRepository.GetPermissionById(permissionId, fitnessClubResult.Value.FitnessClubId);
            if (permission != null)
            {
                return Ok(_mapper.Map<TPermissionDto>(permission));
            }
            return NotFound();
        }

        [HttpPost("{workerId}")]
        public async Task<ActionResult> CreatePermission([FromRoute] string workerId, [FromBody] TCreatePermissionDto model)
        {
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var permission = _mapper.Map<TPermission>(model);
            permission.FitnessClubId = fitnessClubResult.Value.FitnessClubId;

            var createResult = await _permissionRepository.CreatePermission(permission);

            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<TPermissionDto>(createResult.Value));
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpDelete("{permissionId}/{workerId}")]
        public async Task<ActionResult> DeletePermission([FromRoute] string permissionId, [FromRoute] string workerId)
        {
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var deleteResult = await _permissionRepository.DeletePermission(permissionId, fitnessClubResult.Value.FitnessClubId);

            if (deleteResult.IsSuccess)
            {
                return Ok();
            }
            else if (deleteResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(deleteResult.ErrorCombined);
        }
    }
}
