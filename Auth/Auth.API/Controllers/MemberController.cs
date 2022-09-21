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
    public class MemberController : ControllerBase
    {
        private readonly ISpecificUserRepository<Member, RegisterMemberDto> _memeberRepository;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public MemberController(ISpecificUserRepository<Member, RegisterMemberDto> memberRepository, HttpAuthContext httpAuthContext, IMapper mapper)
        {
            _memeberRepository = memberRepository;
            _httpAuthContext = httpAuthContext;
            _mapper = mapper;
        }

        [HttpGet("all")]
        [Authorize(Roles = nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
        {
            return Ok(_mapper.Map<IEnumerable<MemberDto>>(await _memeberRepository.GetAll()));
        }

        [HttpGet()]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<MemberDto>> GetMemeberData()
        {
            if (string.IsNullOrEmpty(_httpAuthContext.UserId))
            {
                return BadRequest("Missing UserId");
            }

            var member = await _memeberRepository.GetById(_httpAuthContext.UserId);

            if (member is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MemberDto>(member));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<MemberDto>> RegisterMember([FromBody] RegisterMemberDto registerDto)
        {
            var memberResult = await _memeberRepository.Register(registerDto);

            if (memberResult.IsSuccess)
            {
                return Ok(_mapper.Map<MemberDto>(memberResult.Value));
            }

            return BadRequest(memberResult.ErrorCombined);
        }

        [HttpPut()]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<MemberDto>> UpdateMember([FromBody] UpdateMemberDto updateDto)
        {
            var memberId = _httpAuthContext.UserId;
            var model = _mapper.Map<Member>(updateDto);
            model.User = _mapper.Map<ApplicationUser>(updateDto);

            var memberResult = await _memeberRepository.UpdateData(memberId, model);

            if (memberResult.IsSuccess)
            {
                return Ok(_mapper.Map<MemberDto>(memberResult.Value));
            }

            return BadRequest(memberResult.ErrorCombined);
        }
    }
}
