using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using TramitesAI.AI.Domain.Dto;
using TramitesAI.AI.Services.Interfaces;
using TramitesAI.Business.Services.Interfaces;

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

        // Endpoint to test Extract Info from a Local File
        [HttpGet("extract-info-from-local")]
        public IActionResult ExtractInfo([FromQuery] string path)
        {
            try
            {
                // Checking for file
                if (!System.IO.File.Exists(path))
                {
                    return NotFound("File not found.");
                }

                // Conversion to MemoryStream
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    fileStream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin); // Moving pointer to the start
                    List<MemoryStream> memoryStreams = new List<MemoryStream>
                    {
                        memoryStream
                    };

                    // Extracting info
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
                return StatusCode(500, $"Error processing file: {ex.Message}");
            }
        }


        // Endpoint to test GoogleDriveSearcherService
        [HttpGet("download-file-from-url")]
        public IActionResult DownloadFileFromURL([FromQuery] string fileName, string msgId)
        {
            if (fileName == null || msgId == null)
            {
                return BadRequest("Not all query params sent");
            }
            string content = "Default text";
            MemoryStream file = _fileSearcher.GetFile(fileName, msgId);

            if (file.CanRead)
            {
                file.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(file);
                content = reader.ReadToEnd();
            }

            return Ok(value: content);
        }

        // Endpoint for integration between Google Drive Downloader and Tesseract
        [HttpGet("download-and-extract-info")]
        public IActionResult DownloadAndExtractInfo([FromQuery] string fileName, string msgId)
        {

            // Download File
            if (fileName == null || msgId == null)
            {
                return BadRequest("Not all query params sent");
            }

            MemoryStream file = _fileSearcher.GetFile(fileName, msgId);
            List<MemoryStream> data = new();
            data.Add(file);

            // Extract info
            List<ExtractedInfoDTO> result = _informationExtractor.extractInfoFromFiles(data);
            string response = "";

            foreach (ExtractedInfoDTO resultDTO in result)
            {
                Console.WriteLine("Confidence: " + resultDTO.Confidence * 100 + "%");
                Console.WriteLine("Text: " + resultDTO.Text);
                response = JsonSerializer.Serialize(result);

            }
            return Ok(value: response);
        }
    }
}
