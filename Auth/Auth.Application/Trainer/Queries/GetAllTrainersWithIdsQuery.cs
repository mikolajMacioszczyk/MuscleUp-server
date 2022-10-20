using Auth.Application.Common.Interfaces;
using Auth.Application.Trainer.Dtos;
using AutoMapper;
using Common.Exceptions;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Trainer.Queries
{
    public record GetAllTrainersWithIdsQuery : IRequest<IEnumerable<TrainerDto>>
    {
        public string UserIds { get; init; }
        public string Separator { get; init; }
    }

    public class GetAllTrainersWithIdsQueryHandler : IRequestHandler<GetAllTrainersWithIdsQuery, IEnumerable<TrainerDto>>
    {
        private readonly ISpecificUserRepository<Domain.Models.Trainer, RegisterTrainerDto> _repository;
        private readonly IMapper _mapper;

        public GetAllTrainersWithIdsQueryHandler(
            ISpecificUserRepository<Domain.Models.Trainer, RegisterTrainerDto> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrainerDto>> Handle(GetAllTrainersWithIdsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserIds)) throw new BadRequestException($"{nameof(request.UserIds)} cannot be empty string");
            if (string.IsNullOrEmpty(request.Separator)) throw new BadRequestException($"{nameof(request.Separator)} cannot be empty string");

            var userIds = request.UserIds.Split(request.Separator);

            var users = await _repository.GetUsersByIds(userIds);

            return _mapper.Map<IEnumerable<TrainerDto>>(users);
        }
    }
}
