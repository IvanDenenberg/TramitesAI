using Microsoft.EntityFrameworkCore;
using System;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repository.Configuration;
using TramitesAI.src.Repository.Domain.Entidades;
using TramitesAI.src.Repository.Interfaces;

namespace TramitesAI.src.Repository.Implementations
{
    public class SolicitudProcesadaRepositorio : IRepositorio<SolicitudProcesada>
    {
        private readonly ConfigDBContext _context;
        public SolicitudProcesadaRepositorio(ConfigDBContext context)
        {
            _context = context;
        }
        public async Task<SolicitudProcesada> Borrar(int id)
        {
            var solicitud = await _context.SolicitudesProcesadas.FindAsync(id);
            if (solicitud == null)
            {
                throw new ApiException(ErrorCode.ERROR_AL_BORRAR);
            }
            _context.SolicitudesProcesadas.Remove(solicitud);
            await _context.SaveChangesAsync();
            return solicitud;
        }

        public async Task<int> Crear(SolicitudProcesada solicitud)
        {
            _context.SolicitudesProcesadas.Add(solicitud);
            await _context.SaveChangesAsync();
            return solicitud.Id;
        }

        public async Task<SolicitudProcesada> LeerPorId(int id)
        {
            SolicitudProcesada solicitud = await _context.SolicitudesProcesadas
                     .Include(sp => sp.Solicitud)
                     .Include(sp => sp.Respuesta)
                     .FirstOrDefaultAsync(sp => sp.Id == id);

            return solicitud == null ? throw new ApiException(ErrorCode.NO_ENCONTRADO) : solicitud;
        }

        public async Task<IEnumerable<SolicitudProcesada>> LeerTodos()
        {
            return await _context.SolicitudesProcesadas
                     .Include(sp => sp.Solicitud)
                     .Include(sp => sp.Respuesta)
                     .ToListAsync();
        }

        public async Task<SolicitudProcesada> Modificar(SolicitudProcesada solicitud)
        {
            if (solicitud == null)
            {
                throw new ApiException(ErrorCode.PARAMETROS_INVALIDOS);
            }

            _context.Entry(solicitud).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.SolicitudesProcesadas.FindAsync(solicitud.Id);
        }
    }
}
