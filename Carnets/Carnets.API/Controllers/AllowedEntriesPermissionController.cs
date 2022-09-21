﻿using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AllowedEntriesPermissionController 
        : SpecificPermissionControllerBase<AllowedEntriesPermission, AllowedEntriesPermissionDto, CreateAllowedEntriesPermissionDto>
    {
        public AllowedEntriesPermissionController(
            IPermissionRepository<AllowedEntriesPermission> permissionRepository, 
            IMapper mapper,
            IFitnessClubHttpService fitnessClubHttpService,
            HttpAuthContext httpAuthContext) 
            : base(permissionRepository, mapper, fitnessClubHttpService, httpAuthContext)
        {
        }
    }
}