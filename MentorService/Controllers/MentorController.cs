using MentorService.Models;
using MentorService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MentorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MentorController : ControllerBase
    {
        private readonly IMentorRepository _repo;
        public MentorController(IMentorRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllMentorsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var mentor = await _repo.GetMentorAsync(id);
            return mentor == null ? NotFound() : Ok(mentor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Mentor mentor)
        {
            await _repo.AddMentorAsync(mentor);
            return Ok(new { message = "Mentor created successfully", mentor.MentorId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Mentor mentor)
        {
            mentor.MentorId = id;
            await _repo.UpdateMentorAsync(mentor);
            return Ok(new { message = "Mentor updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.DeleteMentorAsync(id);
            return Ok(new { message = "Mentor deleted" });
        }
    }
}
