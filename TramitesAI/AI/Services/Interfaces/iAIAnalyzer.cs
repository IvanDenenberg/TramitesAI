using TramitesAI.AI.Domain.Dto;
using TramitesAI.Business.Domain.Dto;

namespace TramitesAI.AI.Services.Interfaces
{
    public interface IAIAnalyzer
    {
        AnalyzedInformationDTO analyzeInformation(List<ExtractedInfoDTO> infoFromFiles, SolicitudDTO requestDTO);
        string determineType(SolicitudDTO requestDTO);
    }
}
