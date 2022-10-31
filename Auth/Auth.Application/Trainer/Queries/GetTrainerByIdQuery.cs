using Auth.Application.Common.Interfaces;
using Auth.Application.Trainer.Dtos;
using AutoMapper;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Trainer.Queries
{
    public record GetTrainerByIdQuery(string TrainerId) : IRequest<TrainerDto> { }

    public class GetTrainerByIdQueryHandler : IRequestHandler<GetTrainerByIdQuery, TrainerDto>
    {
        private readonly ISpecificUserRepository<Domain.Models.Trainer, RegisterTrainerDto> _repository;
        private readonly IMapper _mapper;

        public GetTrainerByIdQueryHandler(
            ISpecificUserRepository<Domain.Models.Trainer, RegisterTrainerDto> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TrainerDto> Handle(GetTrainerByIdQuery request, CancellationToken cancellationToken)
        {
            var member = await _repository.GetById(request.TrainerId);

            if (member is null)
            {
                return null;
            }

            return _mapper.Map<TrainerDto>(member);
        }
    }
}
