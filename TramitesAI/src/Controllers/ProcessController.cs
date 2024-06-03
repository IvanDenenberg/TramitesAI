using Microsoft.AspNetCore.Mvc;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Business.Services.Interfaces;

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
            ResponseDTO response = await _businessService.ProcessAsync(request);
            //TODO Change return
            return Ok(response);
        }

        [HttpGet("getbyid")]
        public IActionResult GetByMsgId([FromRoute] string id)
        {
            ResponseDTO response = _businessService.GetById(id);
            //TODO Change return
            return Ok(response);
        }
    }
}
