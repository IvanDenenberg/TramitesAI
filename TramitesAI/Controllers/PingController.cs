using Microsoft.AspNetCore.Mvc;

namespace TramitesAI.Controllers
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
