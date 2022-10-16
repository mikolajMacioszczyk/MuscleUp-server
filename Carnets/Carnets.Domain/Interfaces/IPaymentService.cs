using Carnets.Domain.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IPaymentService
    {
        Task<string> GetOrCreateCustomer(string userId);

        Task CreateProduct(GympassType gympassType);

        PaymentResult HandlePaidResult(string jsonBody, string signature);

        Task<string> CreateCheckoutSession(string gympassId, string customerId, string gympassTypeId, string successUrl, string cancelUrl);

        Task DeleteProduct(string gympassTypeId);
    }
}
