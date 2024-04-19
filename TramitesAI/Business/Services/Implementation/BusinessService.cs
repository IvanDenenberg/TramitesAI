using TramitesAI.Business.Domain.Dto;
using TramitesAI.Business.Services.Interfaces;
using TramitesAI.Repository.Interfaces;

namespace TramitesAI.Business.Services.Implementation
{
    public class BusinessService : IBusinessService
    {
        private IRepositoryService<> repositoryService;

        public ResponseDTO GetById(string id)
        {
            throw new NotImplementedException();
        }

        public ResponseDTO Process(RequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
