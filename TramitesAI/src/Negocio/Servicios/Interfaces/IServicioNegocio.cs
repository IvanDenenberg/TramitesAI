using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Repository.Domain.Entidades;

namespace TramitesAI.src.Business.Services.Interfaces
{
    public interface IServicioNegocio

    {
        public Task<RespuestaDTO> ProcesarAsync(SolicitudDTO requestDTO);
        public Task<SolicitudProcesada> LeerPorId(int id);
        public Task<IEnumerable<SolicitudProcesada>> LeerTodasSolicitudesProcesadasAsync();
    }
}
