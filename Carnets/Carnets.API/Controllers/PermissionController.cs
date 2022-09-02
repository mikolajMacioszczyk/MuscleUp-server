using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository<AllowedEntriesPermission> _allowedEntriesPermissionRepository;
        private readonly IPermissionRepository<ClassPermission> _classPermissionRepository;
        private readonly IPermissionRepository<TimePermissionEntry> _timePermissionRepository;
        private readonly IMapper _mapper;

        public PermissionController(IMapper mapper,
            IPermissionRepository<AllowedEntriesPermission> allowedEntriesPermissionRepository, 
            IPermissionRepository<ClassPermission> classPermissionRepository, 
            IPermissionRepository<TimePermissionEntry> timePermissionRepository)
        {
            _allowedEntriesPermissionRepository = allowedEntriesPermissionRepository;
            _mapper = mapper;
            _classPermissionRepository = classPermissionRepository;
            _timePermissionRepository = timePermissionRepository;
        }

        #region AllowedEntries

        [HttpGet("allowedEntries/{permissionId}")]
        public async Task<ActionResult<AllowedEntriesPermissionDto>> GetAllowedEntriesPermission([FromRoute] string permissionId) =>
            await GetPermission<AllowedEntriesPermission, AllowedEntriesPermissionDto>(permissionId, _allowedEntriesPermissionRepository);

        [HttpDelete("allowedEntries/{permissionId}")]
        public async Task<ActionResult> DeleteAllowedEntriesPermission([FromRoute] string permissionId) =>
            await DeletePermission(permissionId, _allowedEntriesPermissionRepository);

        #endregion

        #region ClassPermission

        [HttpGet("class/{permissionId}")]
        public async Task<ActionResult<ClassPermissionDto>> GetClassPermission([FromRoute] string permissionId) =>
            await GetPermission<ClassPermission, ClassPermissionDto>(permissionId, _classPermissionRepository);

        [HttpDelete("class/{permissionId}")]
        public async Task<ActionResult> DeleteClassPermission([FromRoute] string permissionId) =>
           await DeletePermission(permissionId, _classPermissionRepository);

        #endregion

        #region TimePermissionEntry

        [HttpGet("timeEntry/{permissionId}")]
        public async Task<ActionResult<TimePermissionEntryDto>> GetTimeEntryPermission([FromRoute] string permissionId) =>
            await GetPermission<TimePermissionEntry, TimePermissionEntryDto>(permissionId, _timePermissionRepository);

        [HttpDelete("timeEntry/{permissionId}")]
        public async Task<ActionResult> DeleteTimeEntryPermission([FromRoute] string permissionId) =>
           await DeletePermission(permissionId, _timePermissionRepository);

        #endregion

        #region Helpers

        private async Task<ActionResult> GetPermission<TPermission, TPermissionDto>(string permissionId,
            IPermissionRepository<TPermission> permissionRepository)
            where TPermission : PermissionBase
        {
            var permission = await permissionRepository.GetPermissionById(permissionId);
            if (permission != null)
            {
                return Ok(_mapper.Map<TPermissionDto>(permission));
            }
            return NotFound();
        }

        public async Task<ActionResult> DeletePermission<TPermission>([FromRoute] string permissionId,
            IPermissionRepository<TPermission> permissionRepository)
            where TPermission : PermissionBase
        {
            var deleteResult = await permissionRepository.DeletePermission(permissionId);

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

        #endregion
    }
}
