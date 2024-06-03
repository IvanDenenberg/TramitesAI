using Microsoft.EntityFrameworkCore;
using TramitesAI.Common.Exceptions;
using TramitesAI.Repository.Configuration;
using TramitesAI.Repository.Domain.Dto;
using TramitesAI.Repository.Interfaces;

namespace TramitesAI.Repository.Implementations
{
    public class TramiteRepositorio : IRepositorio<Tramite>
    {
        private readonly ConfigDBContext _context;
        public TramiteRepositorio(ConfigDBContext context)
        {
            _context = context;
        }
        public async Task<Tramite> Borrar(int id)
        {
            var tramite = await _context.Tramites.FindAsync(id);
            if (tramite == null)
            {
                throw new ApiException(ErrorCode.DELETE_KEY_NOT_FOUND);
            }
            _context.Tramites.Remove(tramite);
            await _context.SaveChangesAsync();
            return tramite;
        }

        public async Task<int> Crear(Tramite tramite)
        {
            _context.Tramites.Add(tramite);
            return await _context.SaveChangesAsync();
        }

        public async Task<Tramite> LeerPorId(int id)
        {
            Tramite tramite = await _context.Tramites
                     .Include(t => t.TramiteDatos)
                     .Include(t => t.TramiteArchivos)
                     .FirstOrDefaultAsync(ta => ta.Id == id);

            return tramite == null ? throw new ApiException(ErrorCode.NOT_FOUND) : tramite;
        }

        public async Task<IEnumerable<Tramite>> LeerTodos()
        {
            return await _context.Tramites
                     .Include(t => t.TramiteDatos)
                     .Include(t => t.TramiteArchivos)
                     .ToListAsync();
        }

        public async Task<Tramite> Modificar(Tramite tramite)
        {
            if (tramite == null)
            {
                throw new ApiException(ErrorCode.INVALID_PARAMS);
            }

            _context.Entry(tramite).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Tramites.FindAsync(tramite.Id);
        }
    }
}
