using AutoMapper;
using Common.Enums;
using Common.Models.Dtos;
using FitnessClubs.Domain.Interfaces;
using FitnessClubs.Domain.Models;
using FitnessClubs.Domain.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClubs.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FitnessClubController : ControllerBase
    {
        private readonly IFitnessClubRepository _fitnessClubRepository;
        private readonly IMapper _mapper;

        public FitnessClubController(IFitnessClubRepository fitnessClubRepository, IMapper mapper)
        {
            _fitnessClubRepository = fitnessClubRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        [Authorize()]
        public async Task<ActionResult<IEnumerable<FitnessClubDto>>> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<FitnessClubDto>>(await _fitnessClubRepository.GetAll()));
        }

        [HttpGet("{fitnessClubId}")]
        [Authorize()]
        public async Task<ActionResult<FitnessClubDto>> GetById([FromRoute] string fitnessClubId)
        {
            var result = await _fitnessClubRepository.GetById(fitnessClubId);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<FitnessClubDto>(result));
        }

        [HttpGet("worker/{workerId}")]
        //[Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<FitnessClubDto>> GetFitnessClubOfWorker([FromRoute] string workerId)
        {
            var getResult = await _fitnessClubRepository.GetFitnessClubOfWorker(workerId);
            if (getResult.IsSuccess)
            {
                return Ok(_mapper.Map<FitnessClubDto>(getResult.Value));
            }
            return BadRequest(getResult.ErrorCombined);
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Administrator))]
        public async Task<ActionResult<FitnessClubDto>> CreateFitnessClub([FromBody] CreateFitnessClubDto model)
        {
            var fitnessClub = _mapper.Map<FitnessClub>(model);
            var createResult = await _fitnessClubRepository.Create(fitnessClub);
            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<FitnessClubDto>(createResult.Value));
            }
            return BadRequest(createResult.ErrorCombined);
        }

        [HttpDelete("{fitnessClubId}")]
        [Authorize(Roles = nameof(RoleType.Administrator))]
        public async Task<ActionResult> DeleteFitnessClub([FromRoute] string fitnessClubId)
        {
            var deleteResult = await _fitnessClubRepository.Delete(fitnessClubId);
            if (deleteResult.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(deleteResult.ErrorCombined);
        }
    }
}
