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
    public class SubscriptionController : ControllerBase
    {
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IGympassService _gympassService;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public SubscriptionController(
            IFitnessClubHttpService fitnessClubHttpService,
            ISubscriptionService subscriptionService,
            IGympassService gamassService,
            HttpAuthContext httpAuthContext,
            IMapper mapper)
        {
            _gympassService = gamassService;
            _fitnessClubHttpService = fitnessClubHttpService;
            _httpAuthContext = httpAuthContext;
            _mapper = mapper;
            _subscriptionService = subscriptionService;
        }

        [HttpGet("byGympass/{gympassId}")]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<SubscriptionDto>> GetAllGympassSubscriptions([FromRoute] string gympassId)
        {
            var memberId = _httpAuthContext.UserId;

            var subscriptions = await _subscriptionService.GetAllGympassSubscriptions(gympassId, memberId);

            return Ok(subscriptions);
        }

        [HttpGet("byGympassAsWorker/{gympassId}/{memberId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<SubscriptionDto>> GetAllGympassSubscriptions([FromRoute] string gympassId, [FromRoute] string memberId)
        {
            var subscriptions = await _subscriptionService.GetAllGympassSubscriptions(gympassId, memberId);

            return Ok(subscriptions);
        }

        [HttpGet("byMember")]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<SubscriptionDto>> GetAllMemberSubscriptions()
        {
            var memberId = _httpAuthContext.UserId;

            var subscriptions = await _subscriptionService.GetAllMemberSubscriptions(memberId);

            return Ok(subscriptions);
        }

        [HttpGet("byMemberAsWorker/{memberId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<SubscriptionDto>> GetAllMemberSubscriptions([FromRoute] string memberId)
        {
            var subscriptions = await _subscriptionService.GetAllMemberSubscriptions(memberId);

            return Ok(subscriptions);
        }

        [HttpGet("{subscriptionId}")]
        [Authorize(Roles = nameof(RoleType.Member) + "," + nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<SubscriptionDto>> GetSubscriptionById([FromRoute] string subscriptionId)
        {
            var subscription = await _subscriptionService.GetSubscriptionById(subscriptionId);

            if (subscription is null)
            {
                return NotFound();
            }

            return Ok(subscription);
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<SubscriptionDto>> CreateGympassWithSubscription([FromBody] CreateGympassSubscriptionDto model)
        {
            var memberId = _httpAuthContext.UserId;
            var subscription = _mapper.Map<Subscription>(model);

            var payResult = await _gympassService.CreateGympassSubscription(memberId, model.GympassId, subscription);

            if (payResult.IsSuccess)
            {
                return Ok(_mapper.Map<SubscriptionDto>(payResult.Value));
            }
            else if (payResult.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(payResult.ErrorCombined);
        }

        [HttpPost("asWorker")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<SubscriptionDto>> CreateGympassWithSubscriptionAsWorker([FromBody] CreateGympassSubscriptionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);
            var subscription = _mapper.Map<Subscription>(model);

            var payResult = await _gympassService.CreateGympassSubscription(fitnessClub.FitnessClubId, model.GympassId, subscription);

            if (payResult.IsSuccess)
            {
                return Ok(_mapper.Map<SubscriptionDto>(payResult.Value));
            }
            else if (payResult.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(payResult.ErrorCombined);
        }
    }
}
