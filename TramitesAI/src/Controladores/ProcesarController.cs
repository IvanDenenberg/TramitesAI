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
        private readonly IServicioNegocio _businessService;

        public ProcesarController(IServicioNegocio businessService)
        {
            _businessService = businessService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Procesa las solicitudes enviadas")]
        [SwaggerResponse(200, "Éxito", typeof(RespuestaDTO))]
        [SwaggerResponse(200, "Tramite invalido", typeof(RespuestaDTO))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(RespuestaErrorDTO))]
        [SwaggerResponse(501, "Modelo no implementado", typeof(RespuestaErrorDTO))]
        public async Task<IActionResult> ProcesarAsync([FromBody] SolicitudDTO request)
        {
            try
            {
                RespuestaDTO respuesta = await _businessService.ProcesarAsync(request);

                return Ok(respuesta);
            } catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, GenerarRespuestaDeError(ex.Codigo, ex.Descripcion));
            }
        }

        [HttpGet("leer-por-id/{id}")]
        [SwaggerOperation(Summary = "Obtiene un elemento por ID")]
        [SwaggerResponse(200, "Éxito", typeof(RespuestaDTO))]
        [SwaggerResponse(404, "No encontrado", typeof(RespuestaErrorDTO))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(RespuestaErrorDTO))]
        public async Task<IActionResult> LeerSolicitudProcesadaPorId(int id)
        {
            try
            {
                SolicitudProcesada respuesta = await _businessService.LeerPorId(id);

                return Ok(respuesta);

            } catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, GenerarRespuestaDeError(ex.Codigo, ex.Descripcion));
            }
        }

        [HttpGet("leer-todas")]
        [SwaggerOperation(Summary = "Obtiene todos los elementos")]
        [SwaggerResponse(200, "Éxito", typeof(IEnumerable<RespuestaDTO>))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(RespuestaErrorDTO))]
        public async Task<IActionResult> LeerTodasSolicitudesProcesadas()
        {
            try
            {
                IEnumerable<SolicitudProcesada> respuesta = await _businessService.LeerTodasSolicitudesProcesadasAsync();

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
