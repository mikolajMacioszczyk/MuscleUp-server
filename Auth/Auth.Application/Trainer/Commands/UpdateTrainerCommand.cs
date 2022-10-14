using Auth.Application.Common.Interfaces;
using Auth.Application.Trainer.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Exceptions;
using Common.Models;
using MediatR;

namespace Auth.Application.Trainer.Commands
{
    public record UpdateTrainerCommand : IRequest<TrainerDto>
    {
        public UpdateTrainerDto UpdateDto { get; init; }
    }

    public class UpdateTrainerCommandHandler : IRequestHandler<UpdateTrainerCommand, TrainerDto>
    {
        private readonly ISpecificUserRepository<Domain.Models.Trainer, RegisterTrainerDto> _repository;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public UpdateTrainerCommandHandler(
            ISpecificUserRepository<Domain.Models.Trainer, RegisterTrainerDto> repository,
            IMapper mapper,
            HttpAuthContext httpAuthContext)
        {
            _repository = repository;
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
        }

        public async Task<TrainerDto> Handle(UpdateTrainerCommand request, CancellationToken cancellationToken)
        {
            var trainerDto = _httpAuthContext.UserId;
            var model = _mapper.Map<Domain.Models.Trainer>(request.UpdateDto);
            model.User = _mapper.Map<ApplicationUser>(request.UpdateDto);

            var trainerResult = await _repository.UpdateData(trainerDto, model);

            if (trainerResult.IsSuccess)
            {
                return _mapper.Map<TrainerDto>(trainerResult.Value);
            }

            throw new BadRequestException(trainerResult.ErrorCombined);
        }
    }
}
