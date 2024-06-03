using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.AI.Services.Implementation
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

        public int DetermineType(SolicitudDTO requestDTO)
        {
            return _iAIAnalyzer.determineType(requestDTO);
        }


        public AnalyzedInformationDTO ProcessInfo(List<MemoryStream> files, SolicitudDTO requestDTO)
        {
            // Extract info from files
            List<ExtractedInfoDTO> infoFromFiles = _iAIInformationExtractor.extractInfoFromFiles(files);

            // Analyze information
            return _iAIAnalyzer.analyzeInformation(infoFromFiles, requestDTO);
        }
    }
}
