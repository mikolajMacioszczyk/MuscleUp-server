using AutoMapper;
using Common.Enums;
using FitnessClubs.Domain.Interfaces;
using FitnessClubs.Domain.Models;
using FitnessClubs.Domain.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClubs.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerEmploymentController : ControllerBase
    {
        private readonly IWorkerEmploymentRepository _workerEmploymentRepository;
        private readonly IMapper _mapper;

        public WorkerEmploymentController(IWorkerEmploymentRepository workerEmploymentRepository, IMapper mapper)
        {
            _workerEmploymentRepository = workerEmploymentRepository;
            _mapper = mapper;
        }

        [HttpGet("{fitnessClubId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<WorkerEmploymentDto>>> GetAllEmploymentsFromFitnessClub([FromRoute] string fitnessClubId)
        {
            var workerEmployment = await _workerEmploymentRepository.GetAllWorkerEmployments(fitnessClubId, false);
            return Ok(_mapper.Map<IEnumerable<WorkerEmploymentDto>>(workerEmployment));
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<WorkerEmploymentDto>> CreateWorkerEmployment(CreateWorkerEmploymentDto model)
        {
            var workerEmployment = _mapper.Map<WorkerEmployment>(model);
            var createResult = await _workerEmploymentRepository.CreateWorkerEmployment(workerEmployment);
            
            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<WorkerEmploymentDto>(createResult.Value));
            }
            else if (createResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(createResult.ErrorCombined);
        }
    }
}
