using Microsoft.Extensions.Configuration;
using Stripe;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly string _secretKey;

        public PaymentService(IConfiguration configuration)
        {
            _secretKey = configuration["Stripe:SecretKey"];
        }

        public async Task<PaymentIntent> CreatePaymentIntentAsync(long amount, string currency)
        {
            StripeConfiguration.ApiKey = _secretKey;

            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" },
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);
            return intent;
        }

        public async Task<Refund> ProcessRefundAsync(string paymentIntentId)
        {
            StripeConfiguration.ApiKey = _secretKey;

            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId,
            };

            var service = new RefundService();
            var refund = await service.CreateAsync(options);
            return refund;
        }
    }
}