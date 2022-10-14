using Auth.Application.Common.Interfaces;
using Auth.Application.Trainer.Dtos;
using AutoMapper;
using MediatR;

namespace Auth.Application.Trainer.Queries
{
    public record GetAllTrainersQuery : IRequest<IEnumerable<TrainerDto>> { }

    public class GetAllTrainersQueryHandler : IRequestHandler<GetAllTrainersQuery, IEnumerable<TrainerDto>>
    {
        private readonly ISpecificUserRepository<Domain.Models.Trainer, RegisterTrainerDto> _repository;
        private readonly IMapper _mapper;

        public GetAllTrainersQueryHandler(ISpecificUserRepository<Domain.Models.Trainer, RegisterTrainerDto> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrainerDto>> Handle(GetAllTrainersQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<TrainerDto>>(await _repository.GetAll());
        }
    }
}
