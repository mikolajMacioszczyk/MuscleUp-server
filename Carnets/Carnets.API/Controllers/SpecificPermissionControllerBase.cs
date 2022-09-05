using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public abstract class SpecificPermissionControllerBase<TPermission, TPermissionDto> : ControllerBase
        where TPermission : PermissionBase
    {
        private readonly IPermissionRepository<TPermission> _permissionRepository;
        private readonly IMapper _mapper;

        protected SpecificPermissionControllerBase(IPermissionRepository<TPermission> permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<TPermissionDto>>> GetAllPermisions()
        {
            return Ok(_mapper.Map<IEnumerable<TPermissionDto>>(await _permissionRepository.GetAll()));
        }

        private async Task<ActionResult> GetPermission(string permissionId)
        {
            var permission = await _permissionRepository.GetPermissionById(permissionId);
            if (permission != null)
            {
                return Ok(_mapper.Map<TPermissionDto>(permission));
            }
            return NotFound();
        }

        private async Task<ActionResult> CreatePermission(TPermission model)
        {
            var createResult = await _permissionRepository.CreatePermission(model);

            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<TPermissionDto>(createResult.Value));
            }

            return BadRequest(createResult.ErrorCombined);
        }

        public async Task<ActionResult> DeletePermission([FromRoute] string permissionId)
        {
            var deleteResult = await _permissionRepository.DeletePermission(permissionId);

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
