namespace TramitesAI.Repository.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepositoryService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(string id);
        Task Create(TEntity entity);
        Task Update(string id, TEntity entity);
        Task Delete(string id);
    }
}
