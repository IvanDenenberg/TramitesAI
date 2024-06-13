using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.AI.Domain.Dto
{
    public class InformacionExtraidaDTO
    {
        public float Confianza { get; set; }
        public string Texto { get; set; }

        public InformacionExtraidaDTO() { }

        public static ExtractedInfoDTOBuilder Builder()
        {
            return new ExtractedInfoDTOBuilder();
        }

        public class ExtractedInfoDTOBuilder
        {
            private InformacionExtraidaDTO dto = new InformacionExtraidaDTO();

            public ExtractedInfoDTOBuilder Confianza(float confidence)
            {
                dto.Confianza = confidence;
                return this;
            }

            public ExtractedInfoDTOBuilder Texto(string text)
            {
                dto.Texto = text;
                return this;
            }

            public InformacionExtraidaDTO Build()
            {
                return dto;
            }
        }
    }
}
