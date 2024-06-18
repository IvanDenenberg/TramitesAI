using Tesseract;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;

namespace TramitesAI.src.AI.Services.Implementation
{
    public class TesseractEngineWrapper : ITesseractEngineWrapper
    {
        private readonly TesseractEngine _engine;

        public TesseractEngineWrapper(TesseractEngine engine)
        {
            _engine = engine;
        }

        public InformacionExtraidaDTO Procesar(byte[] data)
        {
            using (var image = Pix.LoadFromMemory(data))
            {
                var pagina = _engine.Process(image);
                InformacionExtraidaDTO infoDTO = new InformacionExtraidaDTO();
                var texto = pagina.GetText();
                bool contieneLetras = texto.Any(char.IsLetter);
                if (contieneLetras)
                {
                    infoDTO.Texto = texto;
                    infoDTO.Confianza = pagina.GetMeanConfidence();
                }
                else
                {
                    Console.WriteLine("Imagen ilustrativa");
                }
                return infoDTO;
            }
        }
    }
}
