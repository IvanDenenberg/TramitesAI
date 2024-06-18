using Microsoft.IdentityModel.Tokens;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Repository.Domain.Entidades;

namespace TramitesAI.src.AI.Services.Implementation
{
    public class AIHandler : IAIHandler
    {
        private readonly IAnalizadorAI _iAnalizadorAI;
        private readonly IExtractorInformacion _iExtractorInformacion;

        public AIHandler(IAnalizadorAI iAnalizadorAI, IExtractorInformacion iExtractorInformacion)
        {
            _iAnalizadorAI = iAnalizadorAI;
            _iExtractorInformacion = iExtractorInformacion;
        }

        public async Task<int> DeterminarTramiteAsync(string requestDTO)
        {
            // Determinar Tramite utilizando el Analizador
            TramiteDTO asunto = await _iAnalizadorAI.DeterminarTramite(requestDTO);

            return asunto.valor;
        }

        public Task<InformacionAnalizadaDTO> ProcesarInformacion(List<MemoryStream> archivos, SolicitudDTO solicitud, Tramite tramite)
        {
            List<InformacionExtraidaDTO> textoArchivos = new List<InformacionExtraidaDTO>();
            // En caso de que el tramite requiera archivos, se extrae la informacion de ellos
            if (archivos.IsNullOrEmpty() & archivos.Count() > 0)
            {
                // Extraer informacion de los archivos
                textoArchivos = _iExtractorInformacion.ExtraerInformacionDeArchivos(archivos);
            }


            // Analizar la informacion
            return _iAnalizadorAI.AnalizarInformacionAsync(textoArchivos, solicitud, tramite);
        }

    }
}
