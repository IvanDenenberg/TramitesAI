using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.AI.Services.Interfaces
{
    public interface IAIAnalyzer
    {
        AnalyzedInformationDTO analyzeInformation(List<ExtractedInfoDTO> infoFromFiles, SolicitudDTO requestDTO);
        int determineType(SolicitudDTO requestDTO);
    }
}
