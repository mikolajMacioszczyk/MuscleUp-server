using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers.Permission
{
    [ApiController]
    [Route("[controller]")]
    public class PerkPermissionController 
        : SpecificPermissionControllerBase<PerkPermission, PerkPermissionDto, CreatePerkPermissionDto>
    {
        public PerkPermissionController(
            IPermissionService<PerkPermission> permissionService,
            IMapper mapper,
            IFitnessClubHttpService fitnessClubHttpService,
            HttpAuthContext httpAuthContext)
            : base(permissionService, mapper, fitnessClubHttpService, httpAuthContext)
        {
        }
    }
}
