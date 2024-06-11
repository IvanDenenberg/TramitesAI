using Microsoft.IdentityModel.Tokens;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Repository.Domain.Entidades;

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

        public async Task<int> DeterminarTramiteAsync(string requestDTO)
        {
            TramiteDTO asunto = await _iAIAnalyzer.DeterminarTramite(requestDTO);

            return asunto.valor;
        }

        public Task<InformacionAnalizadaDTO> ProcesarInformacion(List<MemoryStream> archivos, SolicitudDTO solicitud, Tramite tramite)
        {
            List<InformacionExtraidaDTO> textoArchivos = new List<InformacionExtraidaDTO>();
            if (archivos.IsNullOrEmpty() & archivos.Count() > 0)
            {
                // Extract info from files
                textoArchivos = _iAIInformationExtractor.extractInfoFromFiles(archivos);
            }


            // Analyze information
            return _iAIAnalyzer.AnalizarInformacionAsync(textoArchivos, solicitud, tramite);
        }

    }
}
