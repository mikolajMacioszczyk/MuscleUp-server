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

        [HttpGet("allowedEntries/{permissionId}")]
        public async Task<ActionResult<AllowedEntriesPermissionDto>> GetAllowedEntriesPermission(string permissionId)
        {
            var permission = await _allowedEntriesPermissionRepository.GetPermissionById(permissionId);
            if (permission != null)
            {
                return Ok(_mapper.Map<AllowedEntriesPermissionDto>(permission));
            }
            return NotFound();
        }

        [HttpGet("class/{permissionId}")]
        public async Task<ActionResult<ClassPermissionDto>> GetClassPermission(string permissionId)
        {
            var permission = await _classPermissionRepository.GetPermissionById(permissionId);
            if (permission != null)
            {
                return Ok(_mapper.Map<ClassPermissionDto>(permission));
            }
            return NotFound();
        }

        [HttpGet("timeEntry/{permissionId}")]
        public async Task<ActionResult<TimePermissionEntryDto>> GetTimeEntryPermission(string permissionId)
        {
            var permission = await _timePermissionRepository.GetPermissionById(permissionId);
            if (permission != null)
            {
                return Ok(_mapper.Map<TimePermissionEntryDto>(permission));
            }
            return NotFound();
        }
    }
}
