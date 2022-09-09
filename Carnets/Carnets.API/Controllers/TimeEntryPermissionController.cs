using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeEntryPermissionController
        : SpecificPermissionControllerBase<TimePermissionEntry, TimePermissionEntryDto, CreateTimePermissionEntryDto>
    {
        public TimeEntryPermissionController(IPermissionRepository<TimePermissionEntry> permissionRepository, IMapper mapper)
            : base(permissionRepository, mapper)
        {
        }
    }
}
