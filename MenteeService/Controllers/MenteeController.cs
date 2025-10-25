using MenteeService.Models;
using MenteeService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MenteeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenteeController : ControllerBase
    {
        private readonly IMenteeRepository _repo;

        public MenteeController(IMenteeRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mentees = await _repo.GetAllMenteesAsync();
            return Ok(mentees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var mentee = await _repo.GetMenteeAsync(id);
            return mentee == null ? NotFound() : Ok(mentee);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Mentee mentee)
        {
            await _repo.AddMenteeAsync(mentee);
            return Ok(new { message = "Mentee created successfully", mentee.MenteeId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Mentee mentee)
        {
            mentee.MenteeId = id;
            await _repo.UpdateMenteeAsync(mentee);
            return Ok(new { message = "Mentee updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.DeleteMenteeAsync(id);
            return Ok(new { message = "Mentee deleted successfully" });
        }
    }
}
