using TramitesAI.AI.Domain.Dto;
using TramitesAI.AI.Services.Interfaces;
using TramitesAI.Business.Domain.Dto;

namespace TramitesAI.AI.Services.Implementation
{
    public class TensorFlowService : IAIAnalyzer
    {
        public AnalyzedInformationDTO analyzeInformation(List<ExtractedInfoDTO> infoFromFiles, SolicitudDTO requestDTO)
        {
            throw new NotImplementedException();
        }

        public string determineType(SolicitudDTO requestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
