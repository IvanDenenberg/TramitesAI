using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.Business.Services.Interfaces
{
    public interface IBusinessService
    {
        public Task<ResponseDTO> ProcessAsync(SolicitudDTO requestDTO);
        public ResponseDTO GetById(string id);
    }
}
