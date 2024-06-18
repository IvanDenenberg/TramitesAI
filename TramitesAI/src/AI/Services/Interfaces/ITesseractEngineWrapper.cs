using Tesseract;
using TramitesAI.src.AI.Domain.Dto;

namespace TramitesAI.src.AI.Services.Interfaces
{
    public interface ITesseractEngineWrapper
    {
        InformacionExtraidaDTO Procesar(byte[] image);
    }
}
