using Microsoft.EntityFrameworkCore;
using TramitesAI.Common.Exceptions;
using TramitesAI.Repository.Configuration;
using TramitesAI.Repository.Domain.Dto;
using TramitesAI.Repository.Interfaces;

namespace TramitesAI.Repository.Implementations
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
                throw new ApiException(ErrorCode.DELETE_KEY_NOT_FOUND);
            }
            _context.Solicitudes.Remove(solicitud);
            await _context.SaveChangesAsync();
            return solicitud;
        }

        public async Task<int> Crear(Solicitud solicitud)
        {
            _context.Solicitudes.Add(solicitud);
            return await _context.SaveChangesAsync();
        }

        public async Task<Solicitud> LeerPorId(int id)
        {
            Solicitud solicitud = await _context.Solicitudes
                     .Include(s => s.SolicitudProcesada)
                     .FirstOrDefaultAsync(ta => ta.Id == id);

            return solicitud == null ? throw new ApiException(ErrorCode.NOT_FOUND) : solicitud;
        }

        public async Task<IEnumerable<Solicitud>> LeerTodos()
        {
            return await _context.Solicitudes
                     .Include(s => s.SolicitudProcesada)
                     .ToListAsync();
        }

        public async Task<Solicitud> Modificar(Solicitud solicitud)
        {
            if (solicitud == null)
            {
                throw new ApiException(ErrorCode.INVALID_PARAMS);
            }

            _context.Entry(solicitud).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Solicitudes.FindAsync(solicitud.Id);
        }
    }
}
