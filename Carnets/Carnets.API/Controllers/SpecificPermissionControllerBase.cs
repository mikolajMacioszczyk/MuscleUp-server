using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public abstract class SpecificPermissionControllerBase<TPermission, TPermissionDto, TCreatePermissionDto> : ControllerBase
        where TPermission : PermissionBase
    {
        private readonly IPermissionRepository<TPermission> _permissionRepository;
        private readonly IMapper _mapper;

        protected SpecificPermissionControllerBase(IPermissionRepository<TPermission> permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TPermissionDto>>> GetAllPermisions()
        {
            return Ok(_mapper.Map<IEnumerable<TPermissionDto>>(await _permissionRepository.GetAll()));
        }

        [HttpGet("{permissionId}")]
        public async Task<ActionResult> GetPermission([FromRoute] string permissionId)
        {
            var permission = await _permissionRepository.GetPermissionById(permissionId);
            if (permission != null)
            {
                return Ok(_mapper.Map<TPermissionDto>(permission));
            }
            return NotFound();
        }

        [HttpPost()]
        public async Task<ActionResult> CreatePermission([FromBody] TCreatePermissionDto model)
        {
            var permission = _mapper.Map<TPermission>(model);
            var createResult = await _permissionRepository.CreatePermission(permission);

            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<TPermissionDto>(createResult.Value));
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpDelete("{permissionId}")]
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
