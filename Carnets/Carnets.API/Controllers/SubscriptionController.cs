using AutoMapper;
using Carnets.Application.Enums;
using Carnets.Application.FitnessClubs.Queries;
using Carnets.Application.Gympasses.Commands;
using Carnets.Application.Payments.Commands;
using Carnets.Application.Subscriptions.Commands;
using Carnets.Application.Subscriptions.Dtos;
using Carnets.Application.Subscriptions.Queries;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public class SubscriptionController : ApiControllerBase
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public SubscriptionController(
            ILogger<SubscriptionController> logger,
            HttpAuthContext httpAuthContext, 
            IMapper mapper)
        {
            _httpAuthContext = httpAuthContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("by-gympass/{gympassId}")]
        [AuthorizeRoles(RoleType.Member, RoleType.Administrator, RoleType.Worker)]
        public async Task<ActionResult<SubscriptionDto>> GetAllGympassSubscriptions([FromRoute] string gympassId)
        {
            var subscriptions = await Mediator.Send(new GetAllGympassSubscriptionsQuery()
            {
                GympassId = gympassId
            });

            return Ok(_mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions));
        }

        [HttpGet("by-member")]
        [AuthorizeRoles(RoleType.Member)]
        public async Task<ActionResult<SubscriptionDto>> GetAllMemberSubscriptions()
        {
            var memberId = _httpAuthContext.UserId;

            var subscriptions = await Mediator.Send(new GetAllMemberSubscriptionsQuery()
            {
                MemberId = memberId
            });

            return Ok(_mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions));
        }

        [HttpGet("by-member-as-worker/{memberId}")]
        [AuthorizeRoles(RoleType.Worker, RoleType.Administrator)]
        public async Task<ActionResult<SubscriptionDto>> GetAllMemberSubscriptions([FromRoute] string memberId)
        {
            var subscriptions = await Mediator.Send(new GetAllMemberSubscriptionsQuery()
            {
                MemberId = memberId
            });

            return Ok(_mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions));
        }

        [HttpGet("{subscriptionId}")]
        [AuthorizeRoles(RoleType.Member, RoleType.Worker, RoleType.Administrator)]
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

            return Ok(_mapper.Map<SubscriptionDto>(subscription));
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<ActionResult<SubscriptionDto>> PaymentWebhook()
        {
            var jsonBody = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var paymentResult = await Mediator.Send(new HandlePaidResultCommand()
            {
                JsonBody = jsonBody,
                Signature = Request.Headers["Stripe-Signature"]
            });

            switch (paymentResult.PaymentStatus)
            {
                case PaymentStatus.SinglePaymentSuccess:
                    var activateResult = await Mediator.Send(new ActivateGympassCommand()
                    {
                        GympassId = paymentResult.PlanId
                    });
                    if (activateResult.IsSuccess)
                    {
                        return Ok();
                    }
                    return BadRequest(activateResult.ErrorCombined);
                case PaymentStatus.SinglePaymentExpired:
                    var deactivationResult = await Mediator.Send(new DeactivateGympassCommand()
                    {
                        GympassId = paymentResult.PlanId
                    });
                    if (deactivationResult.IsSuccess)
                    {
                        return Ok();
                    }
                    return BadRequest(deactivationResult.ErrorCombined);
                case PaymentStatus.SubscriptionPaid:
                    var subscriptionResult = await Mediator.Send(new CreateOrExtendGympassSubscriptionCommand()
                    {
                        GympassId = paymentResult.PlanId,
                        CustomerId = paymentResult.CustomerId,
                        PaymentMethodId = paymentResult.PaymentMethodId
                    });
                    if (subscriptionResult.IsSuccess)
                    {
                        return Ok(subscriptionResult.Value);
                    }
                    return BadRequest(subscriptionResult.ErrorCombined);
                case PaymentStatus.SubscriptionDeleted:
                    // TODO: Implement
                    return Ok();
                default:
                    _logger.LogWarning($"Unchandled webhook event: {paymentResult.PaymentStatus}." +
                        $"\nBody: {jsonBody}");
                    return NoContent();
            }
        }

        [HttpPost("as-worker")]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<SubscriptionDto>> CreateGympassWithSubscriptionAsWorker([FromBody] CreateGympassSubscriptionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });

            var payResult = await Mediator.Send(new CreateOrExtendGympassSubscriptionCommand()
            {
                GympassId = model.GympassId,
                CustomerId = model.CustomerId,
                PaymentMethodId = model.PaymentMethodId
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
