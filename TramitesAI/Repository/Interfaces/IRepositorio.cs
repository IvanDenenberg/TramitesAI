namespace TramitesAI.Repository.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepositorio<TEntity>
    {
        Task<IEnumerable<TEntity>> LeerTodos();
        Task<TEntity> LeerPorId(int id);
        Task<int> Crear(TEntity entidad);
        Task<TEntity> Modificar(TEntity entidad);
        Task<TEntity> Borrar(int id);
    }
}
