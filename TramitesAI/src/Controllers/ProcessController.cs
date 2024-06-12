using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Business.Services.Interfaces;
using TramitesAI.src.Common.Exceptions;

namespace TramitesAI.src.Controllers
{
    // The route of this controller will have the suffix "api/process"
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public ProcessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessAsync([FromBody] SolicitudDTO request)
        {
            try
            {
                RespuestaDTO respuesta = await _businessService.ProcessAsync(request);

                return Ok(respuesta);
            } catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Description);
            }
        }

        [HttpGet("getbyid")]
        public IActionResult GetByMsgId([FromRoute] string id)
        {
            RespuestaDTO response = _businessService.GetById(id);
            //TODO Change return
            return Ok(response);
        }
    }
}
