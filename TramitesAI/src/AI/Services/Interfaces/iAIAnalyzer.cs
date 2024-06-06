using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.AI.Services.Interfaces
{
    public interface IAIAnalyzer
    {
        Task<InformacionAnalizadaDTO> AnalizarInformacionAsync(List<InformacionExtraidaDTO> infoFromFiles, SolicitudDTO requestDTO, int tipo);
        Task<int> DeterminarTipo(string requestDTO);
    }
}
