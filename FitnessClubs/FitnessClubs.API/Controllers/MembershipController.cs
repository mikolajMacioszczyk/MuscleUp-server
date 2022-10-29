using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Models;
using Common.Models.Dtos;
using FitnessClubs.Application.FitnessClubs.Queries;
using FitnessClubs.Application.Memberships.Commands;
using FitnessClubs.Application.Memberships.Dtos;
using FitnessClubs.Application.Memberships.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClubs.API.Controllers
{
    public class MembershipController : ApiControllerBase
    {
        private readonly HttpAuthContext _httpAuthContext;

        public MembershipController(HttpAuthContext httpAuthContext)
        {
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet("{membershipId}")]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<MembershipDto>> GetMembershipById([FromRoute] string membershipId)
        {
            var workerFitnessClub = await Mediator.Send(new GetFitnessClubOfWorkerQuery()
            {
                WorkerId = _httpAuthContext.UserId
            });

            if (!workerFitnessClub.IsSuccess)
            {
                return BadRequest(workerFitnessClub.ErrorCombined);
            }

            var membership = await Mediator.Send(new GetMembershipByIdQuery()
            {
                MemberId = membershipId,
                FitnessClubId = workerFitnessClub.Value.FitnessClubId
            });

            return membership is null ? NotFound() : Ok(membership);
        }

        [HttpGet("{membershipId}/{fitnessClubId}")]
        [AuthorizeRoles(RoleType.Administrator)]
        public async Task<ActionResult<MembershipDto>> GetMembershipByIdAsAdmin(
            [FromRoute] string membershipId, [FromRoute] string fitnessClubId)
        {
            var membership = await Mediator.Send(new GetMembershipByIdQuery()
            {
                MemberId = membershipId,
                FitnessClubId = fitnessClubId
            });

            return membership is null ? NotFound() : Ok(membership);
        }

        [HttpGet("from-fitness-club")]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<IEnumerable<MembershipDto>>> GetAllMembershipsFromFitnessClub()
        {
            var workerFitnessClub = await Mediator.Send(new GetFitnessClubOfWorkerQuery()
            {
                WorkerId = _httpAuthContext.UserId
            });

            if (!workerFitnessClub.IsSuccess)
            {
                return BadRequest(workerFitnessClub.ErrorCombined);
            }

            var memberships = await Mediator.Send(new GetAllMembershipsFromFitnessClubQuery()
            {
                FitnessClubId = workerFitnessClub.Value.FitnessClubId
            });

            return  Ok(memberships);
        }

        [HttpGet("from-fitness-club/{fitnessClubId}")]
        [AuthorizeRoles(RoleType.Administrator)]
        public async Task<ActionResult<IEnumerable<MembershipDto>>> GetAllMembershipsFromFitnessClubAsAdmin(
            [FromRoute] string fitnessClubId)
        {
            var memberships = await Mediator.Send(new GetAllMembershipsFromFitnessClubQuery()
            {
                FitnessClubId = fitnessClubId
            });

            return Ok(memberships);
        }

        [HttpPost()]
        [AuthorizeRoles(RoleType.Administrator, RoleType.Worker, RoleType.Member)]
        public async Task<ActionResult<MembershipDto>> CreateMembership([FromBody] CreateMembershipDto model)
        {
            var createResult = await Mediator.Send(new CreateOrGetMembershipCommand()
            {
                CreateMembershipDto = model
            });

            if (createResult.IsSuccess)
            {
                return Ok(createResult.Value);
            }

            return BadRequest(createResult.ErrorCombined);
        }
    }
}
