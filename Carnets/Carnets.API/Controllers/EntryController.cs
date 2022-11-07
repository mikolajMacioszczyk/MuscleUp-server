using AutoMapper;
using Carnets.Application.Entries.Commands;
using Carnets.Application.Entries.Dtos;
using Carnets.Application.Entries.Queries;
using Carnets.Application.FitnessClubs.Queries;
using Carnets.Application.Members.Queries;
using Common;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public class EntryController : ApiControllerBase
    {
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public EntryController(HttpAuthContext httpAuthContext, IMapper mapper)
        {
            _httpAuthContext = httpAuthContext;
            _mapper = mapper;
        }

        [HttpGet("{entryId}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<EntryDto>> GetEntryById([FromRoute] string entryId)
        {
            var entry = await Mediator.Send(new GetEntryByIdQuery(entryId));

            if (entry != null)
            {
                return Ok(_mapper.Map<EntryDto>(entry));
            }

            return NotFound();
        }

        [HttpGet("by-gympass/{gympassId}")]
        [AuthorizeRoles(RoleType.Member, RoleType.Worker, RoleType.Administrator)]
        public async Task<ActionResult<IEnumerable<EntryDto>>> GetGympassEntries([FromRoute] string gympassId, 
            [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = CommonConsts.DefaultPageSize)
        {
            var entry = await Mediator.Send(new GetEnteredGympassEntriesQuery(gympassId, pageNumber, pageSize));

            return Ok(_mapper.Map<IEnumerable<EntryDto>>(entry));
        }

        [HttpPost("generate-token")]
        [AuthorizeRoles(RoleType.Member)]
        public async Task<ActionResult<GeneratedEndtryTokenDto>> GenerateEntryToken([FromBody] GenerateEntryTokenDto model)
        {
            var tokenGenerationResult = await Mediator.Send(new GenerateEntryTokenCommand(model.GympassId));

            if (tokenGenerationResult.IsSuccess)
            {
                return Ok(tokenGenerationResult.Value);
            }

            return BadRequest(tokenGenerationResult.ErrorCombined);
        }

        [HttpPut()]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult> CreateEntry([FromBody] EntryTokenDto entryTokenDto)
        {
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery()
            {
                WorkerId = _httpAuthContext.UserId
            });

            var entryResult = await Mediator.Send(new EnterGymCommand(entryTokenDto, fitnessClub.FitnessClubId));
        
            if (entryResult.IsSuccess)
            {
                var createdEntry = _mapper.Map<CreatedEntryDto>(entryResult.Value);

                createdEntry.User = await Mediator.Send(new GetMemberByIdQuery(entryResult.Value.Gympass.UserId));

                return Ok(createdEntry);
            }

            return Ok(new FailedEntryDto() { Error = entryResult.ErrorCombined });
        }
    }
}
