using TramitesAI.Repository.Domain.Dto;
using TramitesAI.Repository.Interfaces;

namespace TramitesAI.Repository.Implementations
{
    public class ProcessedCasesRepository : IRepository<ProcessedCasesDTO>
    {
        public Task<string> Create(ProcessedCasesDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProcessedCasesDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ProcessedCasesDTO> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task Update(string id, ProcessedCasesDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
