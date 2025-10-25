using Microsoft.AspNetCore.Mvc;

namespace MentorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("OK");
        }
    }
}
