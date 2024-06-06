using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.AI.Domain.Dto
{
    public class InformacionExtraidaDTO
    {
        public float Confidence { get; set; }
        public string Text { get; set; }

        public InformacionExtraidaDTO() { }

        public static ExtractedInfoDTOBuilder Builder()
        {
            return new ExtractedInfoDTOBuilder();
        }

        public class ExtractedInfoDTOBuilder
        {
            private InformacionExtraidaDTO dto = new InformacionExtraidaDTO();

            public ExtractedInfoDTOBuilder Confidence(float confidence)
            {
                dto.Confidence = confidence;
                return this;
            }

            public ExtractedInfoDTOBuilder Text(string text)
            {
                dto.Text = text;
                return this;
            }

            public InformacionExtraidaDTO Build()
            {
                return dto;
            }
        }
    }
}
