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
        private readonly IWorkerEmploymentService _workerEmploymentService;
        private readonly IMapper _mapper;

        public WorkerEmploymentController(IWorkerEmploymentService workerEmploymentService, IMapper mapper)
        {
            _workerEmploymentService = workerEmploymentService;
            _mapper = mapper;
        }

        [HttpGet("{fitnessClubId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<WorkerEmploymentDto>>> GetAllEmploymentsFromFitnessClub([FromRoute] string fitnessClubId, [FromQuery] bool includeInactive = false)
        {
            var workerEmployment = await _workerEmploymentService.GetAllWorkerEmployments(fitnessClubId, includeInactive);
            return Ok(_mapper.Map<IEnumerable<WorkerEmploymentDto>>(workerEmployment));
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<WorkerEmploymentDto>> CreateWorkerEmployment(CreateWorkerEmploymentDto model)
        {
            var workerEmployment = _mapper.Map<WorkerEmployment>(model);
            var createResult = await _workerEmploymentService.CreateWorkerEmployment(workerEmployment);

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

        [HttpPut("{workerEmploymentId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<WorkerEmploymentDto>> TerminateWorkerEmployment([FromRoute] string workerEmploymentId)
        {
            var terminateResult = await _workerEmploymentService.TerminateWorkerEmployment(workerEmploymentId);

            if (terminateResult.IsSuccess)
            {
                return Ok(_mapper.Map<WorkerEmploymentDto>(terminateResult.Value));
            }
            else if (terminateResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(terminateResult.ErrorCombined);
        }
    }
}
