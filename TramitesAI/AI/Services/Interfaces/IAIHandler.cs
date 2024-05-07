using TramitesAI.AI.Domain.Dto;
using TramitesAI.Business.Domain.Dto;
using TramitesAI.Repository.Domain.Dto;

namespace TramitesAI.AI.Services.Interfaces
{
    public interface IAIHandler
    {
        string DetermineType(RequestDTO requestDTO);
        AnalyzedInformationDTO ProcessInfo(List<MemoryStream> files, RequestDTO requestDTO);
    }
}
