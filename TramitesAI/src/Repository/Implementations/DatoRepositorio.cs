using Microsoft.EntityFrameworkCore;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repository.Configuration;
using TramitesAI.src.Repository.Domain.Entidades;
using TramitesAI.src.Repository.Interfaces;

namespace TramitesAI.src.Repository.Implementations
{
    public class DatoRepositorio : IRepositorio<Dato>
    {
        private readonly ConfigDBContext _context;
        public DatoRepositorio(ConfigDBContext context)
        {
            _context = context;
        }
        public async Task<Dato> Borrar(int id)
        {
            var dato = await _context.Datos.FindAsync(id);
            if (dato == null)
            {
                throw new ApiException(ErrorCode.DELETE_KEY_NOT_FOUND);
            }
            _context.Datos.Remove(dato);
            await _context.SaveChangesAsync();
            return dato;
        }

        public async Task<int> Crear(Dato dato)
        {
            _context.Datos.Add(dato);
            return await _context.SaveChangesAsync();
        }

        public async Task<Dato> LeerPorId(int id)
        {
            Dato dato = await _context.Datos
                     .Include(d => d.TramiteDatos)
                     .FirstOrDefaultAsync(ta => ta.Id == id);

            return dato == null ? throw new ApiException(ErrorCode.NOT_FOUND) : dato;
        }

        public async Task<IEnumerable<Dato>> LeerTodos()
        {
            return await _context.Datos
                     .Include(d => d.TramiteDatos)
                     .ToListAsync();
        }

        public async Task<Dato> Modificar(Dato dato)
        {
            if (dato == null)
            {
                throw new ApiException(ErrorCode.INVALID_PARAMS);
            }

            _context.Entry(dato).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Datos.FindAsync(dato.Id);
        }
    }
}
