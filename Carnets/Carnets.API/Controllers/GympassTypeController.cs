using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Common.Exceptions;
using Common.Models;
using Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // TODO: WrokerId from Token
    public class GympassTypeController : ControllerBase
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly IMapper _mapper;

        public GympassTypeController(IGympassTypeRepository gympassTypeRepository, IMapper mapper, IFitnessClubHttpService fitnessClubHttpService)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _mapper = mapper;
            _fitnessClubHttpService = fitnessClubHttpService;
        }

        [HttpGet("{gympassTypeId}/{workerId}")]
        public async Task<ActionResult<GympassTypeDto>> GetGympassTypeById([FromRoute] string gympassTypeId, [FromRoute] string workerId)
        {
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var gympassType = await _gympassTypeRepository.GetGympassById(gympassTypeId, fitnessClubResult.Value.FitnessClubId);
            if (gympassType != null)
            {
                return Ok(_mapper.Map<GympassTypeDto>(gympassType));
            }
            return NotFound();
        }

        [HttpGet("active/{workerId}")]
        public async Task<ActionResult<IEnumerable<GympassTypeDto>>> GetActiveGympassTypes([FromRoute] string workerId)
        {
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var gympassTypes = await _gympassTypeRepository.GetAllActiveGympassTypes(fitnessClubResult.Value.FitnessClubId);
            
            return Ok(_mapper.Map<IEnumerable<GympassTypeDto>>(gympassTypes));
        }

        [HttpPost("{workerId}")]
        public async Task<ActionResult<GympassTypeDto>> CreateGympassType([FromRoute] string workerId, [FromBody] CreateGympassTypeDto model)
        {
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

        [HttpPut("{gympassTypeId}/{workerId}")]
        public async Task<ActionResult<GympassTypeDto>> UpdateGympassTypeById(
            [FromRoute] string gympassTypeId, [FromRoute] string workerId, [FromBody] UpdateGympassTypeDto model)
        {
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

        [HttpDelete("{gympassTypeId}/{workerId}")]
        public async Task<ActionResult> DeleteGympassType([FromRoute] string gympassTypeId, [FromRoute] string workerId)
        {
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
