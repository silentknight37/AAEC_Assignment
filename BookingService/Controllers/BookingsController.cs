using Microsoft.AspNetCore.Mvc;
using BookingService.Models;
using BookingService.Repositories;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository _repo;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingRepository repo, ILogger<BookingsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var results = await _repo.GetAllBookingsAsync();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var booking = await _repo.GetBookingAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Booking booking)
        {
            await _repo.AddBookingAsync(booking);
            _logger.LogInformation($"Booking created: {booking.BookingId}");
            return Ok(new { booking.BookingId, Message = "Booking created successfully" });
        }
    }
}
