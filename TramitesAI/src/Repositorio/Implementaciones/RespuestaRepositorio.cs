using Microsoft.EntityFrameworkCore;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repository.Configuration;
using TramitesAI.src.Repository.Domain.Entidades;
using TramitesAI.src.Repository.Interfaces;

namespace TramitesAI.src.Repository.Implementations
{
    public class RespuestaRepositorio : IRepositorio<Respuesta>
    {
        private readonly ConfigDBContext _context;
        public RespuestaRepositorio(ConfigDBContext context)
        {
            _context = context;
        }
        public async Task<Respuesta> Borrar(int id)
        {
            var respuesta = await _context.Respuestas.FindAsync(id);
            if (respuesta == null)
            {
                throw new ApiException(ErrorCode.DELETE_KEY_NOT_FOUND);
            }
            _context.Respuestas.Remove(respuesta);
            await _context.SaveChangesAsync();
            return respuesta;
        }

        public async Task<int> Crear(Respuesta respuesta)
        {
            _context.Respuestas.Add(respuesta);
            return await _context.SaveChangesAsync();
        }

        public async Task<Respuesta> LeerPorId(int id)
        {
            Respuesta respuesta = await _context.Respuestas
                     .FirstOrDefaultAsync(ta => ta.Id == id);

            return respuesta == null ? throw new ApiException(ErrorCode.NO_ENCONTRADO) : respuesta;
        }

        public async Task<IEnumerable<Respuesta>> LeerTodos()
        {
            return await _context.Respuestas
                     .ToListAsync();
        }

        public async Task<Respuesta> Modificar(Respuesta respuesta)
        {
            if (respuesta == null)
            {
                throw new ApiException(ErrorCode.PARAMETROS_INVALIDOS);
            }

            _context.Entry(respuesta).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Respuestas.FindAsync(respuesta.Id);
        }
    }
}
