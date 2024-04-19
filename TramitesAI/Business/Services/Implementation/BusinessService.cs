using TramitesAI.Business.Domain.Dto;
using TramitesAI.Business.Services.Interfaces;
using TramitesAI.Repository.Domain.Dto;
using TramitesAI.Repository.Interfaces;

namespace TramitesAI.Business.Services.Implementation
{
    public class BusinessService : IBusinessService
    {
        private IRepository<ProcessedCasesDTO> _processedCasesRepository;
        private IRepository<AssociatedFilesDTO> _associatedFilesRepository;

        public BusinessService(IRepository<ProcessedCasesDTO> processedCasesRepository, IRepository<AssociatedFilesDTO> associatedFilesRepository)
        {
            _processedCasesRepository = processedCasesRepository;
            _associatedFilesRepository = associatedFilesRepository;
        }

        public ResponseDTO GetById(string id)
        {
            throw new NotImplementedException();
        }

        public ResponseDTO Process(RequestDTO requestDTO)
        {
            // Extract info from the request and save it in the database
            SaveNewCase(requestDTO);

            // Extract info from attachments

            // Process Case
            // Get business rules and send the request with the extracted info

            // Update with response
            UpdateCase();
            

            return new ResponseDTO();
        }

        private void UpdateCase()
        {
            throw new NotImplementedException();
        }

        private void SaveNewCase(RequestDTO requestDTO)
        {
            ProcessedCasesDTO processedCase = ProcessedCasesDTO.Builder()
                    //Definir como se genera el id
                    .Id("1")
                    .MsgId(requestDTO.MsgId)
                    .Channel(requestDTO.Channel)
                    .Email(requestDTO.Email)
                    .Request(requestDTO)
                    .CreatedAt(requestDTO.ReceivedDate)
                    .Build();

            _processedCasesRepository.Create(processedCase);

            foreach (var attachment in requestDTO.Attachments)
            {
                AssociatedFilesDTO associatedFilesDTO = AssociatedFilesDTO.Builder()
                    .Id("1")
                    .MsgId(requestDTO.MsgId)
                    .Build();

                _associatedFilesRepository.Create(associatedFilesDTO);
            }
        }
    }
}
