using MessagingService.Models;
using MessagingService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MessagingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _repo;

        public MessageController(IMessageRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _repo.GetAllMessagesAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var message = await _repo.GetMessageAsync(id);
            return message == null ? NotFound() : Ok(message);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var messages = await _repo.GetMessagesByUserAsync(userId);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Message message)
        {
            await _repo.AddMessageAsync(message);
            return Ok(new { message = "Message sent successfully", message.MessageId });
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _repo.MarkAsReadAsync(id);
            return Ok(new { message = "Message marked as read" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.DeleteMessageAsync(id);
            return Ok(new { message = "Message deleted" });
        }
    }
}
