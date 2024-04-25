using TramitesAI.Business.Domain.Dto;
using static TramitesAI.Repository.Domain.Dto.ProcessedCasesDTO;

namespace TramitesAI.Repository.Domain.Dto
{
    public class AssociatedFilesDTO
    {
        public string Id { get; private set; }
        public string Name { get; private set; }

        //public string AssociatedProcedureId { get; private set; }

        private AssociatedFilesDTO() { }

        public static AssociatedFilesDTOBuilder Builder()
        {
            return new AssociatedFilesDTOBuilder();
        }
        public class AssociatedFilesDTOBuilder
        {
            private AssociatedFilesDTO dto = new AssociatedFilesDTO();

            public AssociatedFilesDTOBuilder Id(string id)
            {
                dto.Id = id;
                return this;
            }

            public AssociatedFilesDTOBuilder MsgId(string name)
            {
                dto.Name = name;
                return this;
            }

            /*
            public AssociatedFilesDTOBuilder AssociatedProcedureId(string id)
            {
                dto.AssociatedProcedureId = id;
                return this;
            }
            */

            public AssociatedFilesDTO Build()
            {
                return dto;
            }
        }
    }

}
