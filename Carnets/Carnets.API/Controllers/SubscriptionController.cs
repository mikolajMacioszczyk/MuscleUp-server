using AutoMapper;
using Carnets.Application.Enums;
using Carnets.Application.FitnessClubs.Queries;
using Carnets.Application.Gympasses.Commands;
using Carnets.Application.Payments.Commands;
using Carnets.Application.Subscriptions.Commands;
using Carnets.Application.Subscriptions.Dtos;
using Carnets.Application.Subscriptions.Queries;
using Carnets.Domain.Models;
using Common.BaseClasses;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public class SubscriptionController : ApiControllerBase
    {
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public SubscriptionController(HttpAuthContext httpAuthContext, IMapper mapper)
        {
            _httpAuthContext = httpAuthContext;
            _mapper = mapper;
        }

        [HttpGet("by-gympass/{gympassId}")]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<SubscriptionDto>> GetAllGympassSubscriptions([FromRoute] string gympassId)
        {
            var memberId = _httpAuthContext.UserId;

            var subscriptions = await Mediator.Send(new GetAllGympassSubscriptionsQuery()
            {
                GympassId = gympassId,
                MemberId = memberId
            });

            return Ok(subscriptions);
        }

        [HttpGet("by-gympass-as-worker/{gympassId}/{memberId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<SubscriptionDto>> GetAllGympassSubscriptions([FromRoute] string gympassId, [FromRoute] string memberId)
        {
            var subscriptions = await Mediator.Send(new GetAllGympassSubscriptionsQuery()
            {
                GympassId = gympassId,
                MemberId = memberId
            });

            return Ok(subscriptions);
        }

        [HttpGet("by-member")]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<SubscriptionDto>> GetAllMemberSubscriptions()
        {
            var memberId = _httpAuthContext.UserId;

            var subscriptions = await Mediator.Send(new GetAllMemberSubscriptionsQuery()
            {
                MemberId = memberId
            });

            return Ok(subscriptions);
        }

        [HttpGet("by-member-as-worker/{memberId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<SubscriptionDto>> GetAllMemberSubscriptions([FromRoute] string memberId)
        {
            var subscriptions = await Mediator.Send(new GetAllMemberSubscriptionsQuery()
            {
                MemberId = memberId
            });

            return Ok(subscriptions);
        }

        [HttpGet("{subscriptionId}")]
        [Authorize(Roles = nameof(RoleType.Member) + "," + nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<SubscriptionDto>> GetSubscriptionById([FromRoute] string subscriptionId)
        {
            var subscription = await Mediator.Send(new GetSubscriptionByIdQuery()
            {
                SubscriptionId = subscriptionId
            });

            if (subscription is null)
            {
                return NotFound();
            }

            return Ok(subscription);
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<ActionResult<SubscriptionDto>> PaymentWebhook()
        {
            var paymentResult = await Mediator.Send(new HandlePaidResultCommand()
            {
                JsonBody = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(),
                Signature = Request.Headers["Stripe-Signature"]
            });

            switch (paymentResult.PaymentStatus)
            {
                case PaymentStatus.Success:
                    var activateResult = await Mediator.Send(new ActivateGympassCommand()
                    {
                        GympassId = paymentResult.PlanId
                    });
                    // TODO: Create subscription in later implementation
                    if (activateResult.IsSuccess)
                    {
                        return Ok();
                    }
                    return BadRequest(activateResult.ErrorCombined);
                case PaymentStatus.Expired:
                    var deactivationResult = await Mediator.Send(new DeactivateGympassCommand()
                    {
                        GympassId = paymentResult.PlanId
                    });
                    if (deactivationResult.IsSuccess)
                    {
                        return Ok();
                    }
                    return BadRequest(deactivationResult.ErrorCombined);
                default:
                    return NoContent();
            }
        }

        [HttpPost("as-worker")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<SubscriptionDto>> CreateGympassWithSubscriptionAsWorker([FromBody] CreateGympassSubscriptionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });
            var subscription = _mapper.Map<Subscription>(model);

            var payResult = await Mediator.Send(new CreateGympassSubscriptionCommand()
            {
                GympassId = model.GympassId,
                Subscription = subscription,
            });

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
