using Carnets.Application.Gympasses.Commands;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Helpers
{
    public static class GympassHelper
    {
        public static async Task EnsureGympassActivityStatus(ISender _mediator, IEnumerable<Gympass> gympasses, bool saveChanges = true)
        {
            foreach (var gympass in gympasses)
            {
                await _mediator.Send(new EnsureGympassActivityStatusCommand()
                {
                    Gympass = gympass,
                    SaveChanges = saveChanges
                });
            }
        }
    }
}
