using AutoMapper;
using Carnets.Application.SpecificPermissions.Dtos;
using Carnets.Domain.Models;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassPermissionController
        : SpecificPermissionControllerBase<ClassPermission, ClassPermissionDto, CreateClassPermissionDto>
    {
        public ClassPermissionController(IMapper mapper, HttpAuthContext httpAuthContext)
            : base(mapper, httpAuthContext)
        {
        }
    }
}
