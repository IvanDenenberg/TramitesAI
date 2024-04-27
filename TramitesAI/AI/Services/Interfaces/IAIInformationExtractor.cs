using TramitesAI.AI.Domain.Dto;

namespace TramitesAI.AI.Services.Interfaces
{
    public interface IAIInformationExtractor
    {
        List<ExtractedInfoDTO> extractInfoFromFiles(List<Stream> files);
    }
}
