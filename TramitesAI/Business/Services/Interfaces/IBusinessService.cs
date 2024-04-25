using TramitesAI.Business.Domain.Dto;

namespace TramitesAI.Business.Services.Interfaces
{
    public interface IBusinessService
    {
        public Task<ResponseDTO> ProcessAsync(RequestDTO requestDTO);
        public ResponseDTO GetById(string id);
    }
}
