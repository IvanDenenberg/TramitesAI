using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Business.Services.Interfaces;
using TramitesAI.src.Repository.Domain.Entidades;
using TramitesAI.src.Repository.Interfaces;

namespace TramitesAI.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IAIInformationExtractor _informationExtractor;
        private readonly IFileSearcher _fileSearcher;
        private readonly IRepositorio<Solicitud> _repositorioSolicitud;
        private readonly IRepositorio<SolicitudProcesada> _repositorioSolicitudProcesada;
        private readonly HttpClient _httpClient;

        public TestController(IAIInformationExtractor informationExtractor, IFileSearcher fileSearcher, IRepositorio<Solicitud> repositorioSolicitud, IRepositorio<SolicitudProcesada> repositorioSolicitudProcesada, HttpClient httpClient)
        {
            _informationExtractor = informationExtractor;
            _fileSearcher = fileSearcher;
            _repositorioSolicitud = repositorioSolicitud;
            _repositorioSolicitudProcesada = repositorioSolicitudProcesada;
            _httpClient = httpClient;
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
                    List<InformacionExtraidaDTO> result = _informationExtractor.extractInfoFromFiles(memoryStreams);
                    string response = "";

                    foreach (InformacionExtraidaDTO resultDTO in result)
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
            MemoryStream file = _fileSearcher.ObtenerArchivo(fileName, msgId);

            if (file.CanRead)
            {
                file.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(file);
                content = reader.ReadToEnd();
            }

            return Ok(value: content);
        }

        public class DownloadRequest
        {
            public string MsgId { get; set; }
            public string FileName { get; set; }
        }

        // Endpoint for integration between Google Drive Downloader and Tesseract
        [HttpPost("download-and-extract-info")]
        public IActionResult DownloadAndExtractInfo([FromBody] DownloadRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.FileName) || string.IsNullOrEmpty(request.MsgId))
            {
                return BadRequest("Not all required parameters are provided");
            }

            // Verificar si se proporcionaron ambos valores
            if (string.IsNullOrEmpty(request.MsgId) || string.IsNullOrEmpty(request.FileName))
            {
                return BadRequest("Not all required parameters are provided");
            }

            MemoryStream file = _fileSearcher.ObtenerArchivo(request.FileName, request.MsgId);
            List<MemoryStream> data = new();
            data.Add(file);

            // Extract info
            List<InformacionExtraidaDTO> result = _informationExtractor.extractInfoFromFiles(data);
            string response = "";

            foreach (InformacionExtraidaDTO resultDTO in result)
            {
                Console.WriteLine("Confidence: " + resultDTO.Confidence * 100 + "%");
                Console.WriteLine("Text: " + resultDTO.Text);
                response = JsonSerializer.Serialize(result);

            }
            return Ok(value: response);
        }

        [HttpPost("crear-solicitud")]
        public async Task<IActionResult> CrearSolicitudAsync([FromBody] SolicitudDTO solicitudDTO)
        {
            try
            {
                // Serializa la solicitudDTO a un string JSON
                string solicitudString = JsonSerializer.Serialize(solicitudDTO);

                // Construye la entidad Solicitud a partir del string serializado
                Solicitud solicitudAGuardar = Solicitud.Builder()
                    .MensajeSolicitud(solicitudString)
                    .Build();

                // Guarda la solicitud en el repositorio y obtiene el ID resultante
                int id = await _repositorioSolicitud.Crear(solicitudAGuardar);
                solicitudAGuardar.Id = id;

                // Devuelve un resultado 201 (Created) con la entidad creada
                return Created($"api/crear-solicitud/{solicitudAGuardar.Id}", solicitudAGuardar);
            }
            catch (Exception)
            {
                // Manejo de errores (log, retorno de un mensaje de error, etc.)
                return StatusCode(500, "Ocurrió un error al crear la solicitud.");
            }
        }

        [HttpGet("leer-solicitud/{id}")]
        public async Task<IActionResult> LeerSolicitudAsync(int id)
        {
            Solicitud solicitud = await _repositorioSolicitud.LeerPorId(id);
            return Ok(solicitud);
        }

        [HttpPost("crear-solicitud-procesada")]
        public async Task<IActionResult> CrearSolicitudProcesadaAsync([FromBody] JsonElement solicitudJson)
        {
            try
            {
                // Extraer datos del JSON
                string msgId = solicitudJson.GetProperty("MsgId").GetString();
                string canal = solicitudJson.GetProperty("Channel").GetString();
                string email = solicitudJson.GetProperty("Email").GetString();
                int solicitudId = solicitudJson.GetProperty("SolicitudId").GetInt32();
                DateTime creado = solicitudJson.GetProperty("ReceivedDate").GetDateTime();

                // Construye la entidad Solicitud a partir del string serializado
                SolicitudProcesada solicitudProcesadaAGuardar = SolicitudProcesada.Builder()
                    .MsgId(msgId)
                    .Canal(canal)
                    .Email(email)
                    .Creado(creado)
                    .TipoTramite(1)
                    .Solicitud(await _repositorioSolicitud.LeerPorId(solicitudId))
                    .Build();

                // Guarda la solicitud en el repositorio y obtiene el ID resultante
                int id = await _repositorioSolicitudProcesada.Crear(solicitudProcesadaAGuardar);
                solicitudProcesadaAGuardar.Id = id;

                // Devuelve un resultado 201 (Created) con la entidad creada
                return Created($"api/crear-solicitud/{solicitudProcesadaAGuardar.Id}", solicitudProcesadaAGuardar);
            }
            catch (Exception ex)
            {
                // Manejo de errores (log, retorno de un mensaje de error, etc.)
                return StatusCode(500, "Ocurrió un error al crear la solicitud.");
            }
        }
        [HttpGet("leer-solicitud-procesada/{id}")]
        public async Task<IActionResult> LeerSolicitudProcesadaAsync(int id)
        {
            SolicitudProcesada solicitud = await _repositorioSolicitudProcesada.LeerPorId(id);
            return Ok(solicitud);
        }

        // Endpoint para probar la interaccion entre esta API y la API de Python
        [HttpGet("python-ping")]
        public async Task<IActionResult> PythonPing()
        {
            // Establecer la dirección base de la API externa
            _httpClient.BaseAddress = new Uri("http://127.0.0.1:5000");

            try
            {
                // Hacer la solicitud GET al endpoint /ping
                HttpResponseMessage response = await _httpClient.GetAsync("/ping");
                response.EnsureSuccessStatusCode();

                // Leer el contenido de la respuesta como una cadena
                string responseBody = await response.Content.ReadAsStringAsync();

                // Retornar la respuesta al cliente
                return Ok(responseBody);
            }
            catch (HttpRequestException e)
            {
                // Manejar posibles errores de solicitud
                return StatusCode(500, $"Error en la solicitud: {e.Message}");
            }
        }
    }
}
