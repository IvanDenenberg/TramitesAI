using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.AI.Services.Interfaces
{
    public interface IAIHandler
    {
        int DeterminarTipo(string asunto);
        InformacionAnalizadaDTO ProcesarInformacion(List<MemoryStream> files, SolicitudDTO requestDTO, int tipo);
    }
}
