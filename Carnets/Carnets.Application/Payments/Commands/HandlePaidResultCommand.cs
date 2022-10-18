using Carnets.Application.Interfaces;
using Carnets.Application.Models;
using MediatR;

namespace Carnets.Application.Payments.Commands
{
    public record HandlePaidResultCommand : IRequest<PaymentResult>
    {
        public string JsonBody { get; init; }
        public string Signature { get; init; }
    }

    public class HandlePaidResultCommandHandler : IRequestHandler<HandlePaidResultCommand, PaymentResult>
    {
        private readonly IPaymentService _paymentService;

        public HandlePaidResultCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public Task<PaymentResult> Handle(HandlePaidResultCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_paymentService.HandlePaidResult(request.JsonBody, request.Signature));
        }
    }
}
