using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models.Dtos;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GympassController : ControllerBase
    {
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly IGympassService _gympassService;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public GympassController(IMapper mapper,
            IFitnessClubHttpService fitnessClubHttpService,
            IGympassService gympassService,
            HttpAuthContext httpAuthContext)
        {
            _gympassService = gympassService;
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
            _fitnessClubHttpService = fitnessClubHttpService;
        }

        [HttpGet()]
        [Authorize(Roles = nameof(RoleType.Administrator) + "," + nameof(RoleType.Member))]
        public async Task<ActionResult<IEnumerable<GympassDto>>> GetAll()
        {
            var gympasses = await _gympassService.GetAll();

            return Ok(_mapper.Map<IEnumerable<GympassDto>>(gympasses));
        }

        [HttpGet("from-fitness-club")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<IEnumerable<GympassDto>>> GetAllFromFitnessClub()
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var gympasses = await _gympassService.GetAllFromFitnessClub(fitnessClub.FitnessClubId);

            return Ok(_mapper.Map<IEnumerable<GympassDto>>(gympasses));
        }

        [HttpGet("{gympassId}")]
        [Authorize(Roles = AuthHelper.RoleAll)]
        public async Task<ActionResult<GympassDto>> GetById([FromRoute] string gympassId)
        {
            var gympass = await _gympassService.GetById(gympassId);

            return Ok(_mapper.Map<GympassDto>(gympass));
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<GympassDto>> Create([FromBody] CreateGympassDto model)
        {
            var memberId = _httpAuthContext.UserId;

            var createResult = await _gympassService.CreateGympass(memberId, model.GympassTypeId);

            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<GympassDto>(createResult.Value));
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpPut("cancel/{gympassId}")]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<GympassDto>> CancelGympass([FromRoute] string gympassId)
        {
            var memberId = _httpAuthContext.UserId;
            var result = await _gympassService.CancelGympass(gympassId, memberId);

            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<GympassDto>(result.Value));
            }
            else if (result.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCombined);
        }

        [HttpPut("cancel-as-worker/{gympassId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassDto>> CancelGympassAsWorker([FromRoute] string gympassId)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var result = await _gympassService.CancelGympassByFitnessClub(gympassId, fitnessClub.FitnessClubId);

            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<GympassDto>(result.Value));
            }
            else if (result.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCombined);
        }

        [HttpPut("activate/{gympassId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassDto>> ActivateGympass([FromRoute] string gympassId)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var result = await _gympassService.ActivateGympassByFitnessClub(gympassId, fitnessClub.FitnessClubId);

            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<GympassDto>(result.Value));
            }
            else if (result.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCombined);
        }

        [HttpPut("deactivate/{gympassId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassDto>> DeactivateGympass([FromRoute] string gympassId)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var result = await _gympassService.DeactivateGympassyByFitnessClub(gympassId, fitnessClub.FitnessClubId);

            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<GympassDto>(result.Value));
            }
            else if (result.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCombined);
        }

        [HttpPut("entry/{gympassId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Member))]
        public async Task<ActionResult<GympassDto>> ReduceGympassEntries([FromRoute] string gympassId)
        {
            var result = await _gympassService.ReduceGympassEntries(gympassId);

            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<GympassDto>(result.Value));
            }
            else if (result.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCombined);
        }
    }
}
