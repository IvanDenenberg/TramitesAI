using Microsoft.EntityFrameworkCore;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repositorio.Servicios.Interfaces;
using TramitesAI.src.Repository.Configuration;
using TramitesAI.src.Repository.Domain.Entidades;

namespace TramitesAI.src.Repositorio.Servicios.Implementaciones
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
                throw new ApiException(ErrorCode.ERROR_AL_BORRAR);
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

            return tramite == null ? throw new ApiException(ErrorCode.NO_ENCONTRADO) : tramite;
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
                throw new ApiException(ErrorCode.PARAMETROS_INVALIDOS);
            }

            _context.Entry(tramite).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Tramites.FindAsync(tramite.Id);
        }
    }
}
