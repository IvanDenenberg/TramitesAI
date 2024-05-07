using TramitesAI.AI.Domain.Dto;
using TramitesAI.AI.Services.Interfaces;
using TramitesAI.Business.Domain.Dto;

namespace TramitesAI.AI.Services.Implementation
{
    public class AIHandler : IAIHandler
    {
        private IAIAnalyzer _iAIAnalyzer;
        private IAIInformationExtractor _iAIInformationExtractor;

        public AIHandler(IAIAnalyzer iAIAnalyzer, IAIInformationExtractor iAIInformationExtractor)
        {
            _iAIAnalyzer = iAIAnalyzer;
            _iAIInformationExtractor = iAIInformationExtractor;
        }

        public string DetermineType(RequestDTO requestDTO)
        {
            return _iAIAnalyzer.determineType(requestDTO);
        }


        public AnalyzedInformationDTO ProcessInfo(List<MemoryStream> files, RequestDTO requestDTO)
        {
            // Extract info from files
            List<ExtractedInfoDTO> infoFromFiles = _iAIInformationExtractor.extractInfoFromFiles(files);

            // Analyze information
             return _iAIAnalyzer.analyzeInformation(infoFromFiles, requestDTO);
        }
    }
}
