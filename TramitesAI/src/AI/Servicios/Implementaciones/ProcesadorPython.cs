using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Comun.Servicios.Interfaces;
using TramitesAI.src.Repository.Domain.Entidades;

namespace TramitesAI.src.AI.Services.Implementation
{
    public class ProcesadorPython : IAnalizadorAI
    {
        private readonly IHttpClientWrapper _httpClientWrapper;

        public ProcesadorPython(IHttpClientWrapper httpClientWrapper)
        {
            _httpClientWrapper = httpClientWrapper;
        }

        public async Task<InformacionAnalizadaDTO> AnalizarInformacionAsync(List<InformacionExtraidaDTO> textoArchivos, SolicitudDTO solicitud, Tramite tramite)
        {
            try
            {
                // Definir el objeto contenido fuera del switch
                object contenido = null;
                string pythonEndpoint;

                // Se define el contenido y el endpoint en base al tramite
                switch (tramite.Nombre)
                {
                    case "Denuncia Siniestro":
                        pythonEndpoint = "/denuncia_siniestro";
                        contenido = new
                        {
                            textos = textoArchivos
                        };
                        break;
                    case "Cotizar Poliza Auto":
                        pythonEndpoint = "/poliza_auto";
                        contenido = new
                        {
                            textos = new List<string> { solicitud.Mensaje }
                        };
                        break;
                    case "Carga Presupuestos":
                        pythonEndpoint = "/carga_presupuesto";
                        contenido = new
                        {
                            textos = textoArchivos
                        };
                        break;
                    case "Cotizar Poliza Hogar":
                        pythonEndpoint = "/poliza_hogar";
                        contenido = new
                        {
                            textos = new List<string> { solicitud.Mensaje }
                        };
                        break;
                    default:
                        throw new ApiException(ErrorCode.ERROR_DESCONOCIDO);
                }

                // Hacer la solicitud POST al endpoint /analyze
                HttpResponseMessage respuesta = await _httpClientWrapper.PostAsync(pythonEndpoint, contenido);
                if (respuesta.StatusCode.Equals(HttpStatusCode.NotImplemented))
                {
                    Console.Error.WriteLine("El modelo de Python aun no fue implementado");
                    throw new ApiException(ErrorCode.MODELO_NO_IMPLEMENTADO);
                }

                respuesta.EnsureSuccessStatusCode();

                // Leer el contenido de la respuesta como una cadena
                string cuerpoRespuesta = await respuesta.Content.ReadAsStringAsync();

                // Deserializar solo el campo "resultados" de la respuesta
                JObject jsonResponse = JObject.Parse(cuerpoRespuesta);
                JToken resultadosToken = jsonResponse["resultados"];

                if (resultadosToken == null)
                {
                    throw new ApiException(ErrorCode.JSON_INVALIDO);
                }

                // Deserializar el token de "resultados" a una lista de objetos Resultado
                return resultadosToken[0].ToObject<InformacionAnalizadaDTO>();
            }
            catch (ApiException e)
            {
                Console.WriteLine("Error deserializando el JSON: " + e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deserializando el JSON: " + e.Message);
                if (e is HttpRequestException)
                {
                    throw new ApiException(ErrorCode.HTTP_REQUEST_ERROR);
                } else
                {
                    throw new ApiException(ErrorCode.ERROR_INTERNO_SERVIDOR);
                }
            }
        }

        public async Task<TramiteDTO> DeterminarTramite(string asunto)
        {
            try
            {
                // Crear el objeto que se va a enviar en el POST
                var contenido = new
                {
                    textos = new List<string> { asunto }
                };

                // Hacer la solicitud 
                HttpResponseMessage respuesta = await _httpClientWrapper.PostAsync("/evaluar_asunto", contenido);
                respuesta.EnsureSuccessStatusCode();

                // Leer el contenido de la respuesta como una cadena
                string cuerpoRespuesta = await respuesta.Content.ReadAsStringAsync();

                // Deserializar solo el campo "resultados" de la respuesta
                JObject jsonResponse = JObject.Parse(cuerpoRespuesta);
                JToken resultadosToken = jsonResponse["resultados"];

                if (resultadosToken == null)
                {
                    throw new ApiException(ErrorCode.JSON_INVALIDO);
                }

                // Deserializar el token de "resultados" a una lista de objetos AsuntoDTO
                return new TramiteDTO((int) resultadosToken[0]);
            }
            catch (ApiException e)
            {
                Console.WriteLine("Error deserializando el JSON: " + e.Message);
                throw;
            }
            catch (Exception e)
            {
                if (e is HttpRequestException)
                {
                    throw new ApiException(ErrorCode.HTTP_REQUEST_ERROR);
                }
                else
                {
                    throw new ApiException(ErrorCode.ERROR_INTERNO_SERVIDOR);
                }
            }
        }
    }
}
