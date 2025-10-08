using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        // POST: api/payment/create-payment-intent
        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent()
        {
            // Amount is fixed at 500 INR
            // Stripe expects the amount in the smallest currency unit (e.g., cents, paise)
            long amountInPaise = 500 * 100;

            var paymentIntent = await paymentService.CreatePaymentIntentAsync(amountInPaise, "inr");

            if (paymentIntent == null)
            {
                return BadRequest(new { message = "Failed to create payment intent." });
            }

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
    }
}