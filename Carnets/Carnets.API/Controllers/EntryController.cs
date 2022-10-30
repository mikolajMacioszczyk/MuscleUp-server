using AutoMapper;
using Carnets.Application.Entries.Commands;
using Carnets.Application.Entries.Dtos;
using Carnets.Application.Entries.Queries;
using Carnets.Application.FitnessClubs.Queries;
using Common.BaseClasses;
using Common.Enums;
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

        [HttpGet("generate-token/{gympassId}")]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<EntryTokenDto>> GenerateEntryToken([FromRoute] string gympassId)
        {
            var tokenGenerationResult = await Mediator.Send(new GenerateEntryTokenQuery(gympassId));

            if (tokenGenerationResult.IsSuccess)
            {
                return Ok(tokenGenerationResult.Value);
            }

            return BadRequest(tokenGenerationResult.ErrorCombined);
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult> CreateEntry([FromBody] EntryTokenDto entryTokenDto)
        {
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery()
            {
                WorkerId = _httpAuthContext.UserId
            });

            var entryResult = await Mediator.Send(new CreateGymEntryCommand(entryTokenDto, fitnessClub.FitnessClubId));
        
            if (entryResult.IsSuccess)
            {
                return Ok(_mapper.Map<EntryDto>(entryResult.Value));
            }

            return BadRequest(entryResult.ErrorCombined);
        }
    }
}
