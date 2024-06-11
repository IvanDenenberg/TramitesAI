using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.Business.Services.Interfaces
{
    public interface IBusinessService
    {
        public Task<RespuestaDTO> ProcessAsync(SolicitudDTO requestDTO);
        public RespuestaDTO GetById(string id);
    }
}
