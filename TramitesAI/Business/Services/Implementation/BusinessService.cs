using TramitesAI.AI.Domain.Dto;
using TramitesAI.AI.Services.Interfaces;
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
        private IAIHandler _AIHandler;
        private IFileSearcher _fileSearcher;

        public BusinessService(IRepository<ProcessedCasesDTO> processedCasesRepository, IRepository<AssociatedFilesDTO> associatedFilesRepository, IAIHandler aIHandler, IFileSearcher fileSearcher)
        {
            _processedCasesRepository = processedCasesRepository;
            _associatedFilesRepository = associatedFilesRepository;
            _AIHandler = aIHandler;
            _fileSearcher = fileSearcher;
        }

        public ResponseDTO GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> ProcessAsync(RequestDTO requestDTO)
        {
            // Extract info from the request and save it in the database
            string id = await SaveNewCaseAsync(requestDTO);

            // Get files from storage
            List<FileStream> files = GetFilesFromRequest(requestDTO.Attachments);

            // Get business rules and send the request with the extracted info
            List<BusinessRulesDTO> rules = SearchBusinessRules();

            // Process Case
            // Extract info from attachments and analyze
            AnalyzedInformationDTO analyzedInformation = _AIHandler.ProcessInfo(files, rules, requestDTO);

            //Generate Response
            ResponseDTO responseDTO = GenerateResponse(analyzedInformation);


            // Update with response
            UpdateCase(responseDTO, id, "typeID");
            

            return new ResponseDTO();
        }

        private List<BusinessRulesDTO> SearchBusinessRules()
        {
            throw new NotImplementedException();
        }

        private ResponseDTO GenerateResponse(AnalyzedInformationDTO analyzedInformation)
        {
            throw new NotImplementedException();
        }

        private async void UpdateCase(ResponseDTO responseDTO, string id, string typeID)
        {
            ProcessedCasesDTO processedCasesDTO = await _processedCasesRepository.GetById(id);
            {
                //Add info that was not available before analysis
                processedCasesDTO.Response = responseDTO;
                processedCasesDTO.UpdatedAt = DateTime.Now;
                processedCasesDTO.TypeId = typeID;
            }

            _ = _processedCasesRepository.Update(id, processedCasesDTO);
        }

        private List<FileStream> GetFilesFromRequest(List<string> attachments)
        {
            List<FileStream> files = new List<FileStream>();
            foreach (string attachment in attachments)
            {
                //TODO Add full path
                FileStream file = _fileSearcher.GetFile(attachment);
                files.Add(file);
            }

            return files;
        }


        private async Task<string> SaveNewCaseAsync(RequestDTO requestDTO)
        {
            ProcessedCasesDTO processedCase = ProcessedCasesDTO.Builder()
                    .MsgId(requestDTO.MsgId)
                    .Channel(requestDTO.Channel)
                    .Email(requestDTO.Email)
                    .Request(requestDTO)
                    .CreatedAt(requestDTO.ReceivedDate)
                    .Build();
             
            return await _processedCasesRepository.Create(processedCase);
        }
    }
}
