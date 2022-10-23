using AutoMapper;
using Common.Models;
using Common.Models.Dtos;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Application.Memberships.Dtos;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.Memberships.Commands
{
    public record CreateOrGetMembershipCommand : IRequest<Result<MembershipDto>>
    {
        public CreateMembershipDto CreateMembershipDto { get; init; }
        public bool UserConfirmed { get; init; } = false;
    }

    public class CreateOrGetMembershipCommandHandler : IRequestHandler<CreateOrGetMembershipCommand, Result<MembershipDto>>
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IFitnessClubRepository _fitnessClubRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public CreateOrGetMembershipCommandHandler(
            IMembershipRepository membershipRepository,
            IFitnessClubRepository fitnessClubRepository,
            IAuthService authService,
            IMapper mapper)
        {
            _membershipRepository = membershipRepository;
            _fitnessClubRepository = fitnessClubRepository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<Result<MembershipDto>> Handle(CreateOrGetMembershipCommand request, CancellationToken cancellationToken)
        {
            var membership = _mapper.Map<Membership>(request.CreateMembershipDto);

            var fitnessClub = await _fitnessClubRepository.GetById(membership.FitnessClubId, false);

            if (fitnessClub is null)
            {
                return new Result<MembershipDto>($"{nameof(FitnessClub)} with id {membership.FitnessClubId} does not exists");
            }

            var existing = await _membershipRepository.GetMembershipById(membership.MemberId, membership.FitnessClubId, false);

            if (existing != null)
            {
                return new Result<MembershipDto>(_mapper.Map<MembershipDto>(membership));
            }

            if (!request.UserConfirmed && !(await _authService.DoesMemberExists(request.CreateMembershipDto.MemberId)))
            {
                return new Result<MembershipDto>($"Member with id {request.CreateMembershipDto.MemberId} does not exists");
            }

            var created = await _membershipRepository.CreateMembership(membership);
            await _membershipRepository.SaveChangesAsync();

            return new Result<MembershipDto>(_mapper.Map<MembershipDto>(created));
        }
    }
}
