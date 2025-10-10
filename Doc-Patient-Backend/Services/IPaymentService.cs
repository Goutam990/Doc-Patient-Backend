using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IPaymentService
    {
        Task<(string clientSecret, string paymentIntentId, string ErrorMessage)> CreatePaymentIntentAsync(int appointmentId);
        Task<(bool Succeeded, string ErrorMessage)> ProcessRefundAsync(int appointmentId);
        // The webhook logic will be handled in a dedicated controller that calls the appropriate service methods.
    }
}
