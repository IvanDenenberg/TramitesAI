using TramitesAI.AI.Domain.Dto;
using TramitesAI.Business.Domain.Dto;
using TramitesAI.Repository.Domain.Dto;

namespace TramitesAI.AI.Services.Interfaces
{
    public interface IAIHandler
    {
        AnalyzedInformationDTO ProcessInfo(List<FileStream> files, List<BusinessRulesDTO> rules, RequestDTO requestDTO);
    }
}
