using Carnets.Application.Gympasses.Commands;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Queries
{
    public record GetGympassByIdQuery : IRequest<Gympass>
    {
        public string GympassId { get; init; }
    }

    public class GetGympassByIdQueryHandler : IRequestHandler<GetGympassByIdQuery, Gympass>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISender _mediator;

        public GetGympassByIdQueryHandler(
            IGympassRepository gympassRepository,
            ISender mediator)
        {
            _gympassRepository = gympassRepository;
            _mediator = mediator;
        }

        public async Task<Gympass> Handle(GetGympassByIdQuery request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, false);

            if (gympass != null)
            {
                await _mediator.Send(new EnsureGympassActivityStatusCommand()
                {
                    Gympass = gympass
                });
            }

            return gympass;
        }
    }
}
