using Auth.Application.Common.Interfaces;
using Auth.Application.Trainer.Dtos;
using AutoMapper;
using Common.Exceptions;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Trainer.Commands
{
    public record RegisterTrainerCommand : IRequest<TrainerDto>
    {
        public RegisterTrainerDto RegisterDto { get; init; }
    }

    public class RegisterTrainerCommandHandler : IRequestHandler<RegisterTrainerCommand, TrainerDto>
    {
        private readonly ISpecificUserRepository<Domain.Models.Trainer, RegisterTrainerDto> _repository;
        private readonly IMapper _mapper;

        public RegisterTrainerCommandHandler(ISpecificUserRepository<Domain.Models.Trainer, RegisterTrainerDto> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TrainerDto> Handle(RegisterTrainerCommand request, CancellationToken cancellationToken)
        {
            var trainerResult = await _repository.Register(request.RegisterDto);

            if (trainerResult.IsSuccess)
            {
                return _mapper.Map<TrainerDto>(trainerResult.Value);
            }

            throw new BadRequestException(trainerResult.ErrorCombined);
        }
    }
}
