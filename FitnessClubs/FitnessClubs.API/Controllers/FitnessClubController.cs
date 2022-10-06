using AutoMapper;
using Common.Enums;
using Common.Helpers;
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
        private readonly IFitnessClubService _fitnessClubService;
        private readonly IWorkerEmploymentRepository _workerEmploymentRepository;
        private readonly IMapper _mapper;

        public FitnessClubController(IMapper mapper,
            IFitnessClubService fitnessClubService,
            IWorkerEmploymentRepository workerEmploymentRepository)
        {
            _fitnessClubService = fitnessClubService;
            _mapper = mapper;
            _workerEmploymentRepository = workerEmploymentRepository;
        }

        [HttpGet()]
        [Authorize(Roles = AuthHelper.RoleAll)]
        public async Task<ActionResult<IEnumerable<FitnessClubDto>>> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<FitnessClubDto>>(await _fitnessClubService.GetAll()));
        }

        [HttpGet("{fitnessClubId}")]
        [Authorize(Roles = AuthHelper.RoleAll)]
        public async Task<ActionResult<FitnessClubDto>> GetById([FromRoute] string fitnessClubId)
        {
            var result = await _fitnessClubService.GetById(fitnessClubId);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<FitnessClubDto>(result));
        }

        [HttpGet("worker/{workerId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<FitnessClubDto>> GetFitnessClubOfWorker([FromRoute] string workerId)
        {
            var getResult = await _workerEmploymentRepository.GetFitnessClubOfWorker(workerId, false);
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
            var createResult = await _fitnessClubService.Create(fitnessClub);
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
            var deleteResult = await _fitnessClubService.Delete(fitnessClubId);
            if (deleteResult.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(deleteResult.ErrorCombined);
        }
    }
}
