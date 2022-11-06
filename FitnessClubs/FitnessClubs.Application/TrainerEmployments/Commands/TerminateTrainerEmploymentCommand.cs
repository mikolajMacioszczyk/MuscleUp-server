using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Application.Memberships.Dtos;
using FitnessClubs.Domain.Models;
using MediatR;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace FitnessClubs.Application.TrainerEmployments.Commands
{
    public record TerminateTrainerEmploymentCommand : IRequest<Result<TrainerEmployment>>
    {
        public string EmploymentId { get; init; }
    }

    public class TerminateTrainerEmploymentCommandHandler : IRequestHandler<TerminateTrainerEmploymentCommand, Result<TrainerEmployment>>
    {
        private readonly IEmploymentRepository<TrainerEmployment> _repository;
        private readonly IQueueService _queueService;

        public TerminateTrainerEmploymentCommandHandler(
            IEmploymentRepository<TrainerEmployment> repository, 
            IQueueService queueService)
        {
            _repository = repository;
            _queueService = queueService;
        }

        public async Task<Result<TrainerEmployment>> Handle(TerminateTrainerEmploymentCommand request, CancellationToken cancellationToken)
        {
            var terminateResult = await _repository.TerminateEmployment(request.EmploymentId);

            if (terminateResult.IsSuccess)
            {
                await _repository.SaveChangesAsync();

                var trainerEmployment = terminateResult.Value;
                await _queueService.SendAsync(
                   @object: trainerEmployment,
                   exchangeName: Common.CommonConsts.MembershipExchangeName,
                   routingKey: Common.CommonConsts.TerminatedEmploymentQueueName
                   );
            }

            return terminateResult;
        }
    }
}
