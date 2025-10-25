using CodeReviewService.Models;
using CodeReviewService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CodeReviewService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CodeReviewController : ControllerBase
    {
        private readonly ICodeReviewRepository _repo;

        public CodeReviewController(ICodeReviewRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _repo.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var review = await _repo.GetReviewAsync(id);
            return review == null ? NotFound() : Ok(review);
        }

        [HttpGet("mentee/{menteeId}")]
        public async Task<IActionResult> GetByMentee(string menteeId)
        {
            var reviews = await _repo.GetReviewsByMenteeAsync(menteeId);
            return Ok(reviews);
        }

        [HttpGet("mentor/{mentorId}")]
        public async Task<IActionResult> GetByMentor(string mentorId)
        {
            var reviews = await _repo.GetReviewsByMentorAsync(mentorId);
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CodeReview review)
        {
            await _repo.AddReviewAsync(review);
            return Ok(new { message = "Code review submitted successfully", review.ReviewId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CodeReview review)
        {
            review.ReviewId = id;
            review.ReviewedAt = DateTime.UtcNow;
            await _repo.UpdateReviewAsync(review);
            return Ok(new { message = "Code review updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.DeleteReviewAsync(id);
            return Ok(new { message = "Code review deleted successfully" });
        }
    }
}
