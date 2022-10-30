using Common.Interfaces;
using Common.Models;
using Common.Models.Dtos;
using MediatR;

namespace Carnets.Application.Members.Queries
{
    public record GetMemberByIdQuery(string MemberId) : IRequest<MemberDto>
    {
    }

    public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDto>
    {
        private readonly IAuthService _authService;

        public GetMemberByIdQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<MemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
        {
            var membersData = await _authService.GetAllMembersWithIds(new string[] { request.MemberId });

            if (membersData.IsSuccess)
            {
                return membersData.Value.FirstOrDefault();
            }

            throw new Exception(membersData.ErrorCombined);
        }
    }
}
