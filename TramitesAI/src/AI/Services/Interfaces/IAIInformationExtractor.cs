using TramitesAI.src.AI.Domain.Dto;

namespace TramitesAI.src.AI.Services.Interfaces
{
    public interface IAIInformationExtractor
    {
        List<InformacionExtraidaDTO> extractInfoFromFiles(List<MemoryStream> files);
    }
}
