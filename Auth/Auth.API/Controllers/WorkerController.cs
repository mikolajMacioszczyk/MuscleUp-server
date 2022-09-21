using Auth.Domain.Dtos;
using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using AutoMapper;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly ISpecificUserRepository<Worker, RegisterWorkerDto> _workerRepository;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;
        public WorkerController(ISpecificUserRepository<Worker, RegisterWorkerDto> workerRepository, HttpAuthContext httpAuthContext, IMapper mapper)
        {
            _workerRepository = workerRepository;
            _httpAuthContext = httpAuthContext;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [Authorize(Roles = nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<WorkerDto>>> GetAllWorkers()
        {
            return Ok(_mapper.Map<IEnumerable<WorkerDto>>(await _workerRepository.GetAll()));
        }

        [HttpGet()]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<WorkerDto>> GetWorkerData()
        {
            if (string.IsNullOrEmpty(_httpAuthContext.UserId))
            {
                return BadRequest("Missing UserId");
            }

            var worker = await _workerRepository.GetById(_httpAuthContext.UserId);

            if (worker is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WorkerDto>(worker));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkerDto>> RegisterWorker([FromBody] RegisterWorkerDto registerDto)
        {
            var userResult = await _workerRepository.Register(registerDto);

            if (userResult.IsSuccess)
            {
                return Ok(_mapper.Map<WorkerDto>(userResult.Value));
            }

            return BadRequest(userResult.ErrorCombined);
        }

        [HttpPut()]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<WorkerDto>> UpdateTrainer([FromBody] UpdateWorkerDto updateDto)
        {
            var workerId = _httpAuthContext.UserId;
            var model = _mapper.Map<Worker>(updateDto);
            model.User = _mapper.Map<ApplicationUser>(updateDto);

            var updateResult = await _workerRepository.UpdateData(workerId, model);

            if (updateResult.IsSuccess)
            {
                return Ok(_mapper.Map<WorkerDto>(updateResult.Value));
            }

            return BadRequest(updateResult.ErrorCombined);
        }
    }
}
