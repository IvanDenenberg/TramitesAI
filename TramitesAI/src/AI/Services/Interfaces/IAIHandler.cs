using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.AI.Services.Interfaces
{
    public interface IAIHandler
    {
        int DetermineType(SolicitudDTO requestDTO);
        AnalyzedInformationDTO ProcessInfo(List<MemoryStream> files, SolicitudDTO requestDTO);
    }
}
