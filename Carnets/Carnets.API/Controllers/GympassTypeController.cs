using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GympassTypeController : ControllerBase
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly IMapper _mapper;
        private readonly HttpAuthContext _httpAuthContext;

        public GympassTypeController(
            IGympassTypeRepository gympassTypeRepository, 
            IMapper mapper, 
            IFitnessClubHttpService fitnessClubHttpService, 
            HttpAuthContext authContext)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _mapper = mapper;
            _fitnessClubHttpService = fitnessClubHttpService;
            _httpAuthContext = authContext;
        }

        [HttpGet("{gympassTypeId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassTypeDto>> GetGympassTypeById([FromRoute] string gympassTypeId)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var gympassType = await _gympassTypeRepository.GetGympassById(gympassTypeId, fitnessClubResult.Value.FitnessClubId);
            if (gympassType != null)
            {
                return Ok(_mapper.Map<GympassTypeDto>(gympassType));
            }
            return NotFound();
        }

        [HttpGet("active")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<IEnumerable<GympassTypeDto>>> GetActiveGympassTypes()
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var gympassTypes = await _gympassTypeRepository.GetAllActiveGympassTypes(fitnessClubResult.Value.FitnessClubId);
            
            return Ok(_mapper.Map<IEnumerable<GympassTypeDto>>(gympassTypes));
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassTypeDto>> CreateGympassType([FromBody] CreateGympassTypeDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);
            
            var gympassType = _mapper.Map<GympassType>(model);
            gympassType.FitnessClubId = fitnessClubResult.Value.FitnessClubId;

            var createResult = await _gympassTypeRepository.CreateGympassType(gympassType);
            
            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<GympassTypeDto>(createResult.Value));
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpPut("{gympassTypeId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassTypeDto>> UpdateGympassTypeById([FromRoute] string gympassTypeId, [FromBody] UpdateGympassTypeDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var gympassType = _mapper.Map<GympassType>(model);
            gympassType.GympassTypeId = gympassTypeId;

            var updateResult = await _gympassTypeRepository.UpdateGympassType(gympassType, fitnessClubResult.Value.FitnessClubId);

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
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var deleteResult = await _gympassTypeRepository.DeleteGympassType(gympassTypeId, fitnessClubResult.Value.FitnessClubId);

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
