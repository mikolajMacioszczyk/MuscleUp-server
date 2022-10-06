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
    public class ClassPermissionController
        : SpecificPermissionControllerBase<ClassPermission, ClassPermissionDto, CreateClassPermissionDto>
    {
        public ClassPermissionController(
            IPermissionService<ClassPermission> permissionService, 
            IMapper mapper,
            IFitnessClubHttpService fitnessClubHttpService,
            HttpAuthContext httpAuthContext)
            : base(permissionService, mapper, fitnessClubHttpService, httpAuthContext)
        {
        }
    }
}
