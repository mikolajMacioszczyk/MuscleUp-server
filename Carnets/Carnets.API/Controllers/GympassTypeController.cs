using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GympassTypeController : ControllerBase
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IMapper _mapper;

        public GympassTypeController(IGympassTypeRepository gympassTypeRepository, IMapper mapper)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _mapper = mapper;
        }

        [HttpGet("{gympassTypeId}")]
        public async Task<ActionResult<GympassTypeDto>> GetGympassTypeById([FromRoute] string gympassTypeId)
        {
            var gympassType = await _gympassTypeRepository.GetGympassById(gympassTypeId);
            if (gympassType != null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<GympassTypeDto>(gympassType));
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<GympassTypeDto>>> GetActiveGympassTypes()
        {
            var gympassTypes = await _gympassTypeRepository.GetAllActiveGympassTypes();
            
            return Ok(_mapper.Map<IEnumerable<GympassTypeDto>>(gympassTypes));
        }

        [HttpPost()]
        public async Task<ActionResult<GympassTypeDto>> CreateGympassTypeById([FromBody] CreateGympassTypeDto model)
        {
            var createResult = await _gympassTypeRepository.CreateGympassType(_mapper.Map<GympassType>(model));
            
            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<GympassTypeDto>(createResult.Value));
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpPut("{gympassTypeId}")]
        public async Task<ActionResult<GympassTypeDto>> UpdateGympassTypeById([FromRoute] string gympassTypeId, [FromBody] UpdateGympassTypeDto model)
        {
            var gympassType = _mapper.Map<GympassType>(model);
            gympassType.GympassTypeId = gympassTypeId;

            var updateResult = await _gympassTypeRepository.UpdateGympassType(_mapper.Map<GympassType>(model));

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
    }
}
