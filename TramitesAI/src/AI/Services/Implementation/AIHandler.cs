using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.AI.Services.Implementation
{
    public class AIHandler : IAIHandler
    {
        private readonly IAIAnalyzer _iAIAnalyzer;
        private readonly IAIInformationExtractor _iAIInformationExtractor;

        public AIHandler(IAIAnalyzer iAIAnalyzer, IAIInformationExtractor iAIInformationExtractor)
        {
            _iAIAnalyzer = iAIAnalyzer;
            _iAIInformationExtractor = iAIInformationExtractor;
        }

        public int DeterminarTipo(string requestDTO)
        {
            return _iAIAnalyzer.DeterminarTipo(requestDTO).Result;
        }


        public InformacionAnalizadaDTO ProcesarInformacion(List<MemoryStream> files, SolicitudDTO requestDTO, int tipo)
        {
            // Extract info from files
            List<InformacionExtraidaDTO> infoFromFiles = _iAIInformationExtractor.extractInfoFromFiles(files);

            // Analyze information
            return _iAIAnalyzer.AnalizarInformacionAsync(infoFromFiles, requestDTO, tipo).Result;
        }
    }
}
