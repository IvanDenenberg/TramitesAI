using TramitesAI.src.AI.Domain.Dto;

namespace TramitesAI.src.AI.Services.Interfaces
{
    public interface IExtractorInformacion
    {
        List<InformacionExtraidaDTO> ExtraerInformacionDeArchivos(List<MemoryStream> files);
    }
}
