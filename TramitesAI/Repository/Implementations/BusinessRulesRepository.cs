using TramitesAI.Repository.Domain.Dto;
using TramitesAI.Repository.Interfaces;

namespace TramitesAI.Repository.Implementations
{
    public class BusinessRulesRepository : IRepository<BusinessRulesDTO>
    {
        public Task<string> Create(BusinessRulesDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessRulesDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BusinessRulesDTO> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task Update(string id, BusinessRulesDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
