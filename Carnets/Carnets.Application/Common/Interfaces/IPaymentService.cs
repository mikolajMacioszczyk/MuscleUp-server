using Carnets.Application.Models;
using Carnets.Domain.Models;

namespace Carnets.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<string> GetOrCreateCustomer(string userId);

        Task EnsureProductCreated(GympassType gympassTypeId);

        Task CreateProduct(GympassType gympassType);

        PaymentResult HandlePaidResult(string jsonBody, string signature);

        Task<string> CreateCheckoutSession(string gympassId, string customerId, string gympassTypeId, string successUrl, string cancelUrl);

        Task DeleteProduct(string gympassTypeId);
    }
}
