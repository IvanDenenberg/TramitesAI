using TramitesAI.AI.Domain.Dto;
using TramitesAI.AI.Services.Interfaces;
using TramitesAI.Business.Domain.Dto;
using TramitesAI.Repository.Domain.Dto;

namespace TramitesAI.AI.Services.Implementation
{
    public class AIHandler : IAIHandler
    {
        public AnalyzedInformationDTO ProcessInfo(List<FileStream> files, List<BusinessRulesDTO> rules, RequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
