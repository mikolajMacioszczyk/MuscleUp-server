﻿using Carnets.Application.Gympasses.Helpers;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Queries
{
    public record GetAllGympassesQuery : IRequest<IEnumerable<Gympass>> { }

    public class GetAllGympassesQueryHandler : IRequestHandler<GetAllGympassesQuery, IEnumerable<Gympass>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISender _mediator;

        public GetAllGympassesQueryHandler(
            IGympassRepository gympassRepository,
            ISender mediator)
        {
            _gympassRepository = gympassRepository;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Gympass>> Handle(GetAllGympassesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Gympass> all = await _gympassRepository.GetAll(true);

            await GympassHelper.EnsureGympassActivityStatus(_mediator, all);

            return all;
        }
    }
}
