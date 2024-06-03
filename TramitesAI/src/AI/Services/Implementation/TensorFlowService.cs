using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.AI.Services.Implementation
{
    public class TensorFlowService : IAIAnalyzer
    {
        public AnalyzedInformationDTO analyzeInformation(List<ExtractedInfoDTO> infoFromFiles, SolicitudDTO requestDTO)
        {
            throw new NotImplementedException();
        }

        public int determineType(SolicitudDTO requestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
