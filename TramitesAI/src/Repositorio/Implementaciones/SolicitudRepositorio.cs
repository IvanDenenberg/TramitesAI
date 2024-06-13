using Microsoft.EntityFrameworkCore;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repository.Configuration;
using TramitesAI.src.Repository.Domain.Entidades;
using TramitesAI.src.Repository.Interfaces;

namespace TramitesAI.src.Repository.Implementations
{
    public class SolicitudRepositorio : IRepositorio<Solicitud>
    {
        private readonly ConfigDBContext _context;
        public SolicitudRepositorio(ConfigDBContext context)
        {
            _context = context;
        }
        public async Task<Solicitud> Borrar(int id)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);
            if (solicitud == null)
            {
                throw new ApiException(ErrorCode.ERROR_AL_BORRAR);
            }
            _context.Solicitudes.Remove(solicitud);
            await _context.SaveChangesAsync();
            return solicitud;
        }

        public async Task<int> Crear(Solicitud solicitud)
        {
            _context.Solicitudes.Add(solicitud);
            await _context.SaveChangesAsync();
            return solicitud.Id;
        }

        public async Task<Solicitud> LeerPorId(int id)
        {
            Solicitud solicitud = await _context.Solicitudes
                     .FirstOrDefaultAsync(ta => ta.Id == id);

            return solicitud == null ? throw new ApiException(ErrorCode.NO_ENCONTRADO) : solicitud;
        }

        public async Task<IEnumerable<Solicitud>> LeerTodos()
        {
            return await _context.Solicitudes
                     .ToListAsync();
        }

        public async Task<Solicitud> Modificar(Solicitud solicitud)
        {
            if (solicitud == null)
            {
                throw new ApiException(ErrorCode.PARAMETROS_INVALIDOS);
            }

            _context.Entry(solicitud).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Solicitudes.FindAsync(solicitud.Id);
        }
    }
}
