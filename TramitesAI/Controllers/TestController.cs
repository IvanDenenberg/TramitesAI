using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Net;
using System.Text;
using TramitesAI.AI.Domain.Dto;
using TramitesAI.AI.Services.Interfaces;
using TramitesAI.Business.Services.Interfaces;
using TramitesAI.Common.Exceptions;
using TramitesAI.AI.Services.Implementation;

namespace TramitesAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IAIInformationExtractor _informationExtractor;
        private readonly IFileSearcher _fileSearcher;

        public TestController(IAIInformationExtractor informationExtractor, IFileSearcher fileSearcher)
        {
            _informationExtractor = informationExtractor;
             _fileSearcher = fileSearcher;
        }

        //Endpoint to test GoogleDriveSearcherService
        [HttpGet("extract-info")]
        public IActionResult ExtractInfo([FromQuery]string path)
        {
            try
            {
                // Verifica si el archivo existe
                if (!System.IO.File.Exists(path))
                {
                    return NotFound("El archivo PDF no fue encontrado.");
                }

                // Lee el contenido del archivo PDF en un MemoryStream
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    fileStream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin); // Asegúrate de reiniciar el puntero del MemoryStream al principio
                    List<MemoryStream> memoryStreams = new List<MemoryStream>
                    {
                        memoryStream
                    };

                    // Llama al método que procesa el archivo
                    List<ExtractedInfoDTO> result = _informationExtractor.extractInfoFromFiles(memoryStreams);
                    string response = "";

                    foreach (ExtractedInfoDTO resultDTO in result)
                    {
                        Console.WriteLine(resultDTO.Confidence);
                        Console.WriteLine(resultDTO.Text);
                        response = JsonSerializer.Serialize(result);

                    }
                    return Ok(value: response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el archivo PDF: {ex.Message}");
            }
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
