using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Text;
using TramitesAI.AI.Services.Implementation;
using TramitesAI.Business.Domain.Dto;
using TramitesAI.Business.Services.Interfaces;
using TramitesAI.Common.Exceptions;

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
        public IActionResult DownloadFileFromURL([FromQuery] string fileName, string msgId)
        {
            if (fileName == null || msgId == null)
            {
                return BadRequest("Not all query params sended");
            }
            string content = "Default text";
            MemoryStream file = _fileSearcher.GetFile(fileName, msgId);

            if (file.CanRead)
            {
                file.Seek(0, SeekOrigin.Begin); // Movemos el puntero al principio del stream
                StreamReader reader = new StreamReader(file);
                content = reader.ReadToEnd();
            }

            return Ok(value: content);
        }
    }
}
