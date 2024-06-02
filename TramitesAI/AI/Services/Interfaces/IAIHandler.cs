using TramitesAI.AI.Domain.Dto;
using TramitesAI.Business.Domain.Dto;
using TramitesAI.Repository.Domain.Dto;

namespace TramitesAI.AI.Services.Interfaces
{
    public interface IAIHandler
    {
        int DetermineType(SolicitudDTO requestDTO);
        AnalyzedInformationDTO ProcessInfo(List<MemoryStream> files, SolicitudDTO requestDTO);
    }
}
