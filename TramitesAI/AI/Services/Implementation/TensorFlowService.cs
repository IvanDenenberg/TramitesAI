using TramitesAI.AI.Domain.Dto;
using TramitesAI.AI.Services.Interfaces;
using TramitesAI.Business.Domain.Dto;

namespace TramitesAI.AI.Services.Implementation
{
    public class TensorFlowService : IAIAnalyzer
    {
        public AnalyzedInformationDTO analyzeInformation(List<ExtractedInfoDTO> infoFromFiles, RequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }

        public string determineType(RequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
