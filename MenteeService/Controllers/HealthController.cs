using MenteeService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MenteeService.Controllers
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
