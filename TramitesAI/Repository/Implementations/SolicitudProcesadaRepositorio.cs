using Microsoft.EntityFrameworkCore;
using System;
using TramitesAI.Common.Exceptions;
using TramitesAI.Repository.Configuration;
using TramitesAI.Repository.Domain.Dto;
using TramitesAI.Repository.Interfaces;

namespace TramitesAI.Repository.Implementations
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
                throw new ApiException(ErrorCode.DELETE_KEY_NOT_FOUND);
            }
            _context.SolicitudesProcesadas.Remove(solicitud);
            await _context.SaveChangesAsync();
            return solicitud;
        }

        public async Task<int> Crear(SolicitudProcesada solicitud)
        {
            _context.SolicitudesProcesadas.Add(solicitud);
            return await _context.SaveChangesAsync();
        }

        public async Task<SolicitudProcesada> LeerPorId(int id)
        {
            SolicitudProcesada solicitud = await _context.SolicitudesProcesadas
                     .Include(sp => sp.Solicitud)
                     .Include(sp => sp.Respuesta)
                     .FirstOrDefaultAsync(sp => sp.Id == id);

            return solicitud == null ? throw new ApiException(ErrorCode.NOT_FOUND) : solicitud;
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
                throw new ApiException(ErrorCode.INVALID_PARAMS);
            }

            _context.Entry(solicitud).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.SolicitudesProcesadas.FindAsync(solicitud.Id);
        }
    }
}
