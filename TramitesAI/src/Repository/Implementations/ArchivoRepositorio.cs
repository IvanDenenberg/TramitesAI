using Microsoft.EntityFrameworkCore;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repository.Configuration;
using TramitesAI.src.Repository.Domain.Entidades;
using TramitesAI.src.Repository.Interfaces;

namespace TramitesAI.src.Repository.Implementations
{
    public class ArchivoRepositorio : IRepositorio<Archivo>
    {
        private readonly ConfigDBContext _context;
        public ArchivoRepositorio(ConfigDBContext context)
        {
            _context = context;
        }
        public async Task<Archivo> Borrar(int id)
        {
            var archivo = await _context.Archivos.FindAsync(id);
            if (archivo == null)
            {
                throw new ApiException(ErrorCode.DELETE_KEY_NOT_FOUND);
            }
            _context.Archivos.Remove(archivo);
            await _context.SaveChangesAsync();
            return archivo;
        }

        public async Task<int> Crear(Archivo archivo)
        {
            _context.Archivos.Add(archivo);
            return await _context.SaveChangesAsync();
        }

        public async Task<Archivo> LeerPorId(int id)
        {
            Archivo archivo = await _context.Archivos
                     .Include(a => a.TramiteArchivos)
                     .FirstOrDefaultAsync(ta => ta.Id == id);

            return archivo == null ? throw new ApiException(ErrorCode.NOT_FOUND) : archivo;
        }

        public async Task<IEnumerable<Archivo>> LeerTodos()
        {
            return await _context.Archivos
                     .Include(a => a.TramiteArchivos)
                     .ToListAsync();
        }

        public async Task<Archivo> Modificar(Archivo archivo)
        {
            if (archivo == null)
            {
                throw new ApiException(ErrorCode.INVALID_PARAMS);
            }

            _context.Entry(archivo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Archivos.FindAsync(archivo.Id);
        }
    }
}
