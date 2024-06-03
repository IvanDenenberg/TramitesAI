using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAI.src.AI.Domain.Dto
{
    public class ExtractedInfoDTO
    {
        public float Confidence { get; set; }
        public string Text { get; set; }

        public ExtractedInfoDTO() { }

        public static ExtractedInfoDTOBuilder Builder()
        {
            return new ExtractedInfoDTOBuilder();
        }

        public class ExtractedInfoDTOBuilder
        {
            private ExtractedInfoDTO dto = new ExtractedInfoDTO();

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

            public ExtractedInfoDTO Build()
            {
                return dto;
            }
        }
    }
}
