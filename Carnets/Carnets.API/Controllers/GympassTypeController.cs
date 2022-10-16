using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Common;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GympassTypeController : ControllerBase
    {
        private readonly IGympassTypeService _gympassTypeService;
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly IMapper _mapper;
        private readonly HttpAuthContext _httpAuthContext;

        public GympassTypeController(
            IGympassTypeService gympassTypeService, 
            IMapper mapper, 
            IFitnessClubHttpService fitnessClubHttpService, 
            HttpAuthContext authContext)
        {
            _gympassTypeService = gympassTypeService;
            _mapper = mapper;
            _fitnessClubHttpService = fitnessClubHttpService;
            _httpAuthContext = authContext;
        }

        [HttpGet("{gympassTypeId}")]
        [Authorize(Roles = AuthHelper.RoleAll)]
        public async Task<ActionResult<GympassTypeDto>> GetGympassTypeById([FromRoute] string gympassTypeId)
        {
            var gympassType = await _gympassTypeService.GetGympassTypeWithPermissionsById(gympassTypeId);
            if (gympassType != null)
            {
                return Ok(_mapper.Map<GympassTypeDto>(gympassType));
            }
            return NotFound();
        }

        [HttpGet("active-as-worker")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<IEnumerable<GympassTypeDto>>> GetActiveGympassTypesAsWorker(
            [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = CommonConsts.DefaultPageSize)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var gympassTypes = await _gympassTypeService.GetAllGympassTypesWithPermissions(fitnessClub.FitnessClubId, 
                onlyActive: true, pageNumber, pageSize);
            
            return Ok(_mapper.Map<IEnumerable<GympassTypeDto>>(gympassTypes));
        }

        [HttpGet("active/{fitnessClubId}")]
        [Authorize(Roles = nameof(RoleType.Member) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<GympassTypeDto>>> GetActiveGympassTypes([FromRoute] string fitnessClubId,
            [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = CommonConsts.DefaultPageSize)
        {
            await _fitnessClubHttpService.EnsureFitnessClubExists(fitnessClubId);

            var gympassTypes = await _gympassTypeService.GetAllGympassTypesWithPermissions(fitnessClubId, 
                onlyActive: true, pageNumber, pageSize);

            return Ok(_mapper.Map<IEnumerable<GympassTypeDto>>(gympassTypes));
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassTypeDto>> CreateGympassType([FromBody] CreateGympassTypeDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);
            
            var gympassType = _mapper.Map<GympassType>(model);
            gympassType.FitnessClubId = fitnessClub.FitnessClubId;

            var createResult = await _gympassTypeService.CreateGympassType(gympassType, model.ClassPermissions, model.PerkPermissions);
            
            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<GympassTypeDto>(createResult.Value));
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpPut("{gympassTypeId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassTypeDto>> UpdateGympassType([FromRoute] string gympassTypeId, [FromBody] UpdateGympassTypeDto model)
        {
            var gympassType = _mapper.Map<GympassType>(model);
            gympassType.GympassTypeId = gympassTypeId;

            var updateResult = await _gympassTypeService.UpdateGympassType(gympassType);

            if (updateResult.IsSuccess)
            {
                return Ok(_mapper.Map<GympassTypeDto>(updateResult.Value));
            }
            else if (updateResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(updateResult.ErrorCombined);
        }

        [HttpPut("with-permissions/{gympassTypeId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassTypeDto>> UpdateGympassTypeWithPermissions([FromRoute] string gympassTypeId, [FromBody] UpdateGympassTypeWithPermissionsDto model)
        {
            var gympassType = _mapper.Map<GympassType>(model);
            gympassType.GympassTypeId = gympassTypeId;

            var updateResult = await _gympassTypeService.UpdateGympassTypeWithPermissions(gympassType, model.ClassPermissions, model.PerkPermissions);

            if (updateResult.IsSuccess)
            {
                return Ok(_mapper.Map<GympassTypeDto>(updateResult.Value));
            }
            else if (updateResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(updateResult.ErrorCombined);
        }

        [HttpDelete("{gympassTypeId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult> DeleteGympassType([FromRoute] string gympassTypeId)
        {
            var deleteResult = await _gympassTypeService.DeleteGympassType(gympassTypeId);

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
    }
}
