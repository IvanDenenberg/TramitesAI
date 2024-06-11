using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Repository.Domain.Entidades;

namespace TramitesAI.src.AI.Services.Interfaces
{
    public interface IAIAnalyzer
    {
        Task<InformacionAnalizadaDTO> AnalizarInformacionAsync(List<InformacionExtraidaDTO> infoFromFiles, SolicitudDTO requestDTO, Tramite tipo);
        Task<TramiteDTO> DeterminarTramite(string requestDTO);
    }
}
