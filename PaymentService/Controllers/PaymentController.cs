using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;
using PaymentService.Repositories;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _repo;

        public PaymentController(IPaymentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _repo.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var payment = await _repo.GetPaymentAsync(id);
            return payment == null ? NotFound() : Ok(payment);
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetByBooking(string bookingId)
        {
            var payments = await _repo.GetPaymentsByBookingAsync(bookingId);
            return Ok(payments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Payment payment)
        {
            await _repo.AddPaymentAsync(payment);
            return Ok(new { message = "Payment record created successfully", payment.PaymentId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Payment payment)
        {
            payment.PaymentId = id;
            await _repo.UpdatePaymentAsync(payment);
            return Ok(new { message = "Payment updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.DeletePaymentAsync(id);
            return Ok(new { message = "Payment deleted successfully" });
        }
    }
}
