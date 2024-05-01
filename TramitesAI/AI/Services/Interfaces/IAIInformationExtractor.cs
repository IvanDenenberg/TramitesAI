using TramitesAI.AI.Domain.Dto;

namespace TramitesAI.AI.Services.Interfaces
{
    public interface IAIInformationExtractor
    {
        ExtractedInfoDTO extractInfoFromFiles(List<Stream> files);
    }
}
