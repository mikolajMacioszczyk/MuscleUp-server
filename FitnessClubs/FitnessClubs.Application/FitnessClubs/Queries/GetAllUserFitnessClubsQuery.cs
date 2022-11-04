using Common.Enums;
using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.FitnessClubs.Queries
{
    public record GetAllUserFitnessClubsQuery : IRequest<IEnumerable<FitnessClub>> { }

    public class GetAllUserFitnessClubsQueryHandler : IRequestHandler<GetAllUserFitnessClubsQuery, IEnumerable<FitnessClub>>
    {
        private readonly IFitnessClubRepository _fitnessClubRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IEmploymentRepository<WorkerEmployment> _workerEmploymentRepository;
        private readonly IEmploymentRepository<TrainerEmployment> _trainerEmploymentRepository;
        private readonly HttpAuthContext _httpAuthContext;

        public GetAllUserFitnessClubsQueryHandler(
            IFitnessClubRepository repository,
            HttpAuthContext httpAuthContext,
            IMembershipRepository membershipRepository,
            IEmploymentRepository<WorkerEmployment> workerEmploymentRepository, 
            IEmploymentRepository<TrainerEmployment> trainerEmploymentRepository)
        {
            _fitnessClubRepository = repository;
            _httpAuthContext = httpAuthContext;
            _membershipRepository = membershipRepository;
            _workerEmploymentRepository = workerEmploymentRepository;
            _trainerEmploymentRepository = trainerEmploymentRepository;
        }

        public Task<IEnumerable<FitnessClub>> Handle(GetAllUserFitnessClubsQuery request, CancellationToken cancellationToken)
        {
            switch (_httpAuthContext.UserRole)
            {
                case RoleType.Administrator:
                    return GetAllFitnessClubsAsAdmin();
                case RoleType.Owner:
                    return GetAllFitnessClubsAsOwner();
                case RoleType.Worker:
                    return GetAllFitnessClubsAsWorker();
                case RoleType.Member:
                    return GetAllFitnessClubsAsMember();
                case RoleType.Trainer:
                    return GetAllFitnessClubsAsTrainer();
                default:
                    throw new ArgumentOutOfRangeException(nameof(_httpAuthContext.UserRole));
            }
        }

        private Task<IEnumerable<FitnessClub>> GetAllFitnessClubsAsAdmin()
        {
            return _fitnessClubRepository.GetAll(false);
        }

        private Task<IEnumerable<FitnessClub>> GetAllFitnessClubsAsOwner()
        {
            var ownerId = _httpAuthContext.UserId;
            return _fitnessClubRepository.GetOwnerFitnessClubs(ownerId, onlyActive: true, asTracking: false);
        }

        private Task<IEnumerable<FitnessClub>> GetAllFitnessClubsAsWorker()
        {
            return _workerEmploymentRepository.GetAllFitnessClubsOfEmployee(
                _httpAuthContext.UserId, 
                onlyActive: true, 
                asTracking: false);
        }

        private async Task<IEnumerable<FitnessClub>> GetAllFitnessClubsAsMember()
        {
            var memberId = _httpAuthContext.UserId;

            // get memberships of worker
            var memberships = await _membershipRepository.GetAllMembershipsByMember(memberId, false);

            var fitnessClubsIds = memberships.Select(m => m.FitnessClubId);

            // get fitness clubs by ids
            return await _fitnessClubRepository.GetBatchByIds(fitnessClubsIds, false);
        }

        private Task<IEnumerable<FitnessClub>> GetAllFitnessClubsAsTrainer()
        {
            return _trainerEmploymentRepository.GetAllFitnessClubsOfEmployee(
                _httpAuthContext.UserId,
                onlyActive: true,
                asTracking: false);
        }
    }
}
