using TramitesAI.Repository.Domain.Dto;
using TramitesAI.Repository.Interfaces;

namespace TramitesAI.Repository.Implementations
{
    public class AssociatedFilesRepository : IRepository<AssociatedFilesDTO>
    {
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssociatedFilesDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<AssociatedFilesDTO> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task Update(string id, AssociatedFilesDTO entity)
        {
            throw new NotImplementedException();
        }

        Task<string> IRepository<AssociatedFilesDTO>.Create(AssociatedFilesDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}

