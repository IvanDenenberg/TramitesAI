using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Repository.Domain.Entidades;

namespace TramitesAI.src.AI.Services.Interfaces
{
    public interface IAIHandler
    {
        Task<int> DeterminarTramiteAsync(string asunto);
        Task<InformacionAnalizadaDTO> ProcesarInformacion(List<MemoryStream> files, SolicitudDTO requestDTO, Tramite tipo);
    }
}
