using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Route("api/payments")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // POST: api/payments/create-intent/{appointmentId}
        [HttpPost("create-intent/{appointmentId}")]
        public async Task<IActionResult> CreatePaymentIntent(int appointmentId)
        {
            var (clientSecret, paymentIntentId, errorMessage) = await _paymentService.CreatePaymentIntentAsync(appointmentId);

            if (errorMessage != null)
            {
                return BadRequest(new { Message = errorMessage });
            }

            return Ok(new { ClientSecret = clientSecret, PaymentIntentId = paymentIntentId });
        }
    }
}
