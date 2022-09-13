using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassPermissionController
        : SpecificPermissionControllerBase<ClassPermission, ClassPermissionDto, CreateClassPermissionDto>
    {
        public ClassPermissionController(
            IPermissionRepository<ClassPermission> permissionRepository, 
            IMapper mapper,
            IFitnessClubHttpService fitnessClubHttpService)
            : base(permissionRepository, mapper, fitnessClubHttpService)
        {
        }
    }
}
