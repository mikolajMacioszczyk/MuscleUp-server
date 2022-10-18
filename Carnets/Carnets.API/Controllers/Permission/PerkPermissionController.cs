using AutoMapper;
using Carnets.Application.SpecificPermissions.Dtos;
using Carnets.Domain.Models;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers.Permission
{
    [ApiController]
    [Route("[controller]")]
    public class PerkPermissionController 
        : SpecificPermissionControllerBase<PerkPermission, PerkPermissionDto, CreatePerkPermissionDto>
    {
        public PerkPermissionController(IMapper mapper, HttpAuthContext httpAuthContext)
            : base(mapper, httpAuthContext)
        {
        }
    }
}
