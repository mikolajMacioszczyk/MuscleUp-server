using AutoMapper;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Helpers;
using Common.Models.Dtos;
using FitnessClubs.Application.FitnessClubs.Commands;
using FitnessClubs.Application.FitnessClubs.Dtos;
using FitnessClubs.Application.FitnessClubs.Queries;
using FitnessClubs.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClubs.API.Controllers
{
    public class FitnessClubController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public FitnessClubController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet()]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<IEnumerable<FitnessClubDto>>> GetAll()
        {
            var all = await Mediator.Send(new GetAllFitnessClubsQuery());
            return Ok(_mapper.Map<IEnumerable<FitnessClubDto>>(all));
        }

        [HttpGet("{fitnessClubId}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<FitnessClubDto>> GetById([FromRoute] string fitnessClubId)
        {
            var result = await Mediator.Send(new GetFitnessClubByIdQuery() { FitnessClubId = fitnessClubId});
            if (result is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<FitnessClubDto>(result));
        }

        [HttpGet("worker/{workerId}")]
        [AuthorizeRoles(RoleType.Worker, RoleType.Administrator)]
        public async Task<ActionResult<FitnessClubDto>> GetFitnessClubOfWorker([FromRoute] string workerId)
        {
            
            var getResult = await Mediator.Send(new GetFitnessClubOfWorkerQuery() { WorkerId = workerId });
            if (getResult.IsSuccess)
            {
                return Ok(_mapper.Map<FitnessClubDto>(getResult.Value));
            }
            return BadRequest(getResult.ErrorCombined);
        }

        [HttpPost()]
        [AuthorizeRoles(RoleType.Administrator, RoleType.Owner)]
        public async Task<ActionResult<FitnessClubDto>> CreateFitnessClub([FromBody] CreateFitnessClubDto model)
        {
            var command = new CreateFitnessClubCommand()
            {
                FitnessClub = _mapper.Map<FitnessClub>(model)
            };

            var createResult = await Mediator.Send(command);
            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<FitnessClubDto>(createResult.Value));
            }
            return BadRequest(createResult.ErrorCombined);
        }

        [HttpDelete("{fitnessClubId}")]
        [AuthorizeRoles(RoleType.Administrator, RoleType.Owner)]
        public async Task<ActionResult> DeleteFitnessClub([FromRoute] string fitnessClubId)
        {
            var command = new DeleteFitnessClubCommand()
            {
                FitnessClubId = fitnessClubId
            };

            var deleteResult = await Mediator.Send(command);
            if (deleteResult.IsSuccess)
            {
                return NoContent();
            }
            else if (deleteResult.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }
            else if (deleteResult.Errors.Contains(Common.CommonConsts.Unauthorized))
            {
                return Unauthorized();
            }
            return BadRequest(deleteResult.ErrorCombined);
        }
    }
}
