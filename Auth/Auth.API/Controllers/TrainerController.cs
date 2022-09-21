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
    public class TrainerController : ControllerBase
    {
        private readonly ISpecificUserRepository<Trainer, RegisterTrainerDto> _trainerRepository;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public TrainerController(ISpecificUserRepository<Trainer, RegisterTrainerDto> trainerRepository, HttpAuthContext httpAuthContext, IMapper mapper)
        {
            _trainerRepository = trainerRepository;
            _httpAuthContext = httpAuthContext;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [Authorize(Roles = nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<TrainerDto>>> GetAllTrainers()
        {
            return Ok(_mapper.Map<IEnumerable<TrainerDto>>(await _trainerRepository.GetAll()));
        }

        [HttpGet()]
        [Authorize(Roles = nameof(RoleType.Trainer))]
        public async Task<ActionResult<TrainerDto>> GetTrainerData()
        {
            if (string.IsNullOrEmpty(_httpAuthContext.UserId))
            {
                return BadRequest("Missing UserId");
            }

            var trainer = await _trainerRepository.GetById(_httpAuthContext.UserId);

            if (trainer is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TrainerDto>(trainer));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<TrainerDto>> RegisterTrainer([FromBody] RegisterTrainerDto registerDto)
        {
            var userResult = await _trainerRepository.Register(registerDto);

            if (userResult.IsSuccess)
            {
                return Ok(_mapper.Map<TrainerDto>(userResult.Value));
            }

            return BadRequest(userResult.ErrorCombined);
        }

        [HttpPut()]
        [Authorize(Roles = nameof(RoleType.Trainer))]
        public async Task<ActionResult<TrainerDto>> UpdateTrainer([FromBody] UpdateTrainerDto updateDto)
        {
            var trainerId = _httpAuthContext.UserId;
            var model = _mapper.Map<Trainer>(updateDto);
            model.User = _mapper.Map<ApplicationUser>(updateDto);

            var updateResult = await _trainerRepository.UpdateData(trainerId, model);

            if (updateResult.IsSuccess)
            {
                return Ok(_mapper.Map<TrainerDto>(updateResult.Value));
            }

            return BadRequest(updateResult.ErrorCombined);
        }
    }
}
