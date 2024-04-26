using Microsoft.AspNetCore.Mvc;
using System.IO;
using TramitesAI.Business.Domain.Dto;
using TramitesAI.Business.Services.Interfaces;

namespace TramitesAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IFileSearcher _fileSearcher;

        public TestController(IFileSearcher fileSearcher)
        {
            _fileSearcher = fileSearcher;
        }

        //Endpoint to test GoogleDriveSearcherService
        [HttpGet("download-file-from-url")]
        public IActionResult DownloadFileFromURL([FromRoute] string url) 
        {
            FileStream file = _fileSearcher.GetFile(url);

            return Ok(file.Name);
        }
    }
}
