using Microsoft.AspNetCore.Mvc;

namespace TramitesAI.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            Console.WriteLine("Pong");
            return Ok("Pong");
        }
    }
}
