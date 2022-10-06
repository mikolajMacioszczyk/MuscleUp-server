using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeEntryPermissionController
        : SpecificPermissionControllerBase<TimePermissionEntry, TimePermissionEntryDto, CreateTimePermissionEntryDto>
    {
        public TimeEntryPermissionController(
            IPermissionService<TimePermissionEntry> permissionService, 
            IMapper mapper,
            IFitnessClubHttpService fitnessClubHttpService,
            HttpAuthContext httpAuthContext)
            : base(permissionService, mapper, fitnessClubHttpService, httpAuthContext)
        {
        }
    }
}
