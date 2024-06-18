using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Business.Services.Interfaces;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repository.Domain.Entidades;

namespace TramitesAI.src.Controllers
{
    // The route of this controller will have the suffix "api/process"
    [Route("api/[controller]")]
    [ApiController]
    public class ProcesarController : ControllerBase
    {
        private readonly IServicioNegocio _servicioNegocio;

        public ProcesarController(IServicioNegocio businessService)
        {
            _servicioNegocio = businessService;
        }

        /// <summary>
        /// Procesa las solicitudes enviadas
        /// </summary>
        /// <param name="request">La solicitud a procesar</param>
        /// <returns>La respuesta del procesamiento</returns>
        /// <response code="200">Éxito / Trámite inválido</response>
        /// <response code="400">Error ejecutando HTTP request / JSON inválido</response>
        /// <response code="404">Archivo no encontrado / Trámite no encontrado</response>
        /// <response code="500">Error interno del servidor / Error al descargar el archivo</response>
        /// <response code="501">Modelo no implementado</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Procesa las solicitudes enviadas")]
        [SwaggerResponse(200, "Éxito", typeof(RespuestaDTO))]
        [SwaggerResponse(200, "Tramite invalido", typeof(RespuestaDTO))]
        [SwaggerResponse(400, "Error ejecutando HTTP request", typeof(RespuestaErrorDTO))]
        [SwaggerResponse(400, "JSON Invalido", typeof(RespuestaErrorDTO))]
        [SwaggerResponse(404, "Archivo no encontrado", typeof(RespuestaErrorDTO))]
        [SwaggerResponse(404, "Tramite no encontrado", typeof(RespuestaErrorDTO))]
        [SwaggerResponse(500, "Error al descargar el archivo", typeof(RespuestaErrorDTO))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(RespuestaErrorDTO))]
        [SwaggerResponse(501, "Modelo no implementado", typeof(RespuestaErrorDTO))]
        public async Task<IActionResult> ProcesarAsync([FromBody] SolicitudDTO request)
        {
            try
            {
                RespuestaDTO respuesta = await _servicioNegocio.ProcesarAsync(request);

                return Ok(respuesta);
            } catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, GenerarRespuestaDeError(ex.Codigo, ex.Descripcion));
            }
        }

        /// <summary>
        /// Obtiene un elemento por ID
        /// </summary>
        /// <param name="id">El ID del elemento a obtener</param>
        /// <returns>La respuesta de la solicitud</returns>
        /// <response code="200">Éxito</response>
        /// <response code="404">No encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("leer-por-id/{id}")]
        [SwaggerOperation(Summary = "Obtiene un elemento por ID")]
        [SwaggerResponse(200, "Éxito", typeof(RespuestaDTO))]
        [SwaggerResponse(404, "No encontrado", typeof(RespuestaErrorDTO))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(RespuestaErrorDTO))]
        public async Task<IActionResult> LeerSolicitudProcesadaPorId(int id)
        {
            try
            {
                SolicitudProcesada respuesta = await _servicioNegocio.LeerPorId(id);

                return Ok(respuesta);

            } catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, GenerarRespuestaDeError(ex.Codigo, ex.Descripcion));
            }
        }

        /// <summary>
        /// Obtiene todos los elementos
        /// </summary>
        /// <returns>La lista de todos los elementos procesados</returns>
        /// <response code="200">Éxito</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("leer-todas")]
        [SwaggerOperation(Summary = "Obtiene todos los elementos")]
        [SwaggerResponse(200, "Éxito", typeof(IEnumerable<RespuestaDTO>))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(RespuestaErrorDTO))]
        public async Task<IActionResult> LeerTodasSolicitudesProcesadas()
        {
            try
            {
                IEnumerable<SolicitudProcesada> respuesta = await _servicioNegocio.LeerTodasSolicitudesProcesadasAsync();

                return Ok(respuesta);

            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, GenerarRespuestaDeError(ex.Codigo, ex.Descripcion));
            }
        }

        public static object GenerarRespuestaDeError(string codigoError, string descripcionError)
        {
            return RespuestaErrorDTO.Builder()
                .Codigo(codigoError)
                .Descripcion(descripcionError)
                .Build();
        }
    }
}
