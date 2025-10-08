using Stripe;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreatePaymentIntentAsync(long amount, string currency);
        Task<Refund> ProcessRefundAsync(string paymentIntentId);
    }
}