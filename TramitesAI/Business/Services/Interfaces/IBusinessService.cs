using TramitesAI.Business.Domain.Dto;

namespace TramitesAI.Business.Services.Interfaces
{
    public interface IBusinessService
    {
        public ResponseDTO Process(RequestDTO requestDTO);
        public ResponseDTO GetById(string id);
    }
}
