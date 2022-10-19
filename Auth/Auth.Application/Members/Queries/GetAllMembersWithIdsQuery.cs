using Auth.Application.Common.Interfaces;
using Auth.Application.Members.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Exceptions;
using MediatR;

namespace Auth.Application.Members.Queries
{
    public record GetAllMembersWithIdsQuery : IRequest<IEnumerable<MemberDto>>
    {
        public string UserIds { get; init; }
        public string Separator { get; init; }
    }

    public class GetAllMembersWithIdsQueryHandler : IRequestHandler<GetAllMembersWithIdsQuery, IEnumerable<MemberDto>>
    {
        private readonly ISpecificUserRepository<Member, RegisterMemberDto> _repository;
        private readonly IMapper _mapper;

        public GetAllMembersWithIdsQueryHandler(
            ISpecificUserRepository<Member, RegisterMemberDto> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberDto>> Handle(GetAllMembersWithIdsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserIds)) throw new BadRequestException($"{nameof(request.UserIds)} cannot be empty string");
            if (string.IsNullOrEmpty(request.Separator)) throw new BadRequestException($"{nameof(request.Separator)} cannot be empty string");

            var userIds = request.UserIds.Split(request.Separator);

            var users = await _repository.GetUsersByIds(userIds);

            return _mapper.Map<IEnumerable<MemberDto>>(users);
        }
    }
}
