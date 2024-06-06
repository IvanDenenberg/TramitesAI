using Newtonsoft.Json;
using System.Text;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Common.Exceptions;

namespace TramitesAI.src.AI.Services.Implementation
{
    public class ProcesadorPython : IAIAnalyzer
    {
        private readonly HttpClient _httpClient;
        private Uri URL_Python = new("http://127.0.0.1:5000");

        public ProcesadorPython(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = URL_Python;
        }

        public async Task<InformacionAnalizadaDTO> AnalizarInformacionAsync(List<InformacionExtraidaDTO> infoFromFiles, SolicitudDTO requestDTO, int tipo)
        {
            try
            {
                // Crear el objeto que se va a enviar en el POST
                var contenido = new
                {
                    InformacionDeArchivos = infoFromFiles,
                    Request = requestDTO
                };

                // Serializar el objeto a JSON
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(contenido),
                    Encoding.UTF8,
                    "application/json"
                );

                // Hacer la solicitud POST al endpoint /analyze
                HttpResponseMessage response = await _httpClient.PostAsync("/analyze", jsonContent);
                response.EnsureSuccessStatusCode();

                // Leer el contenido de la respuesta como una cadena
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserializar la respuesta a un objeto AnalyzedInformationDTO
                var analyzedInformation = JsonConvert.DeserializeObject<InformacionAnalizadaDTO>(responseBody);

                return analyzedInformation;
            }
            
            catch (Exception e)
            {
                if (e is HttpRequestException)
                {
                    throw new ApiException(ErrorCode.HTTP_REQUEST_ERROR);
                } else
                {
                    throw new ApiException(ErrorCode.INTERNAL_SERVER_ERROR);
                }
            }
        }

        public async Task<int> DeterminarTipo(string asunto)
        {
            try
            {
                // Crear el objeto que se va a enviar en el POST
                var contenido = new
                {
                    textos = new List<string> { asunto }
                };

                // Serializar el objeto a JSON
                var jsonContenido = new StringContent(
                    JsonConvert.SerializeObject(contenido),
                    Encoding.UTF8,
                    "application/json"
                );

                // Hacer la solicitud GET al endpoint /ping
                HttpResponseMessage response = await _httpClient.PostAsync("/determinarTipo", jsonContenido);
                response.EnsureSuccessStatusCode();

                // Leer el contenido de la respuesta como una cadena
                string responseBody = await response.Content.ReadAsStringAsync();

                return 1;

            }
            catch (Exception e)
            {
                if (e is HttpRequestException)
                {
                    throw new ApiException(ErrorCode.HTTP_REQUEST_ERROR);
                }
                else
                {
                    throw new ApiException(ErrorCode.INTERNAL_SERVER_ERROR);
                }
            }
        }
    }
}
