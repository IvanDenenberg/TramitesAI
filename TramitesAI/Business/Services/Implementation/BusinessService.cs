using TramitesAI.AI.Domain.Dto;
using TramitesAI.AI.Services.Interfaces;
using TramitesAI.Business.Domain.Dto;
using TramitesAI.Business.Services.Interfaces;
using TramitesAI.Common.Exceptions;
using TramitesAI.Repository.Domain.Dto;
using TramitesAI.Repository.Interfaces;

namespace TramitesAI.Business.Services.Implementation
{
    public class BusinessService : IBusinessService
    {
        private IRepository<ProcessedCasesDTO> _processedCasesRepository;
        private IAIHandler _AIHandler;
        private IFileSearcher _fileSearcher;

        public BusinessService(IRepository<ProcessedCasesDTO> processedCasesRepository, IAIHandler aIHandler, IFileSearcher fileSearcher)
        {
            _processedCasesRepository = processedCasesRepository;
            _AIHandler = aIHandler;
            _fileSearcher = fileSearcher;
        }

        public ResponseDTO GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> ProcessAsync(RequestDTO requestDTO)
        {
            try
            {
                // Determine the type and validity
                string type = DetermineType(requestDTO);

                // Extract info from the request and save it in the database
                string id = await SaveNewCaseAsync(requestDTO, type);

                // Get files from external storage
                List<MemoryStream> files = GetFilesFromRequest(requestDTO.Attachments, requestDTO.MsgId);
                // Process Case
                // Extract info from attachments and analyze
                AnalyzedInformationDTO analyzedInformation = _AIHandler.ProcessInfo(files, requestDTO);

                //Generate Response
                ResponseDTO responseDTO = GenerateResponse(analyzedInformation);

                // Update with response
                UpdateCase(responseDTO, id, type);

                return new ResponseDTO();
            }
            catch (Exception)
            {
                throw; 
            }
        }

        private string DetermineType(RequestDTO requestDTO)
        {
            // Return id_tramite
            return _AIHandler.DetermineType(requestDTO);
        }

        private ResponseDTO GenerateResponse(AnalyzedInformationDTO analyzedInformation)
        {
            throw new ApiException(ErrorCode.UNKNOWN_ERROR);
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

        private List<MemoryStream> GetFilesFromRequest(List<string> attachments, string msgId)
        {
            try
            {
                List<MemoryStream> files = new();
                foreach (string attachment in attachments)
                {
                    MemoryStream file = _fileSearcher.GetFile(attachment, msgId);
                    files.Add(file);
                }

                return files;
            }
            catch (ApiException)
            {
                throw;
            }
        }

        private async Task<string> SaveNewCaseAsync(RequestDTO requestDTO, string type)
        {
            ProcessedCasesDTO processedCase = ProcessedCasesDTO.Builder()
                    .MsgId(requestDTO.MsgId)
                    .Channel(requestDTO.Channel)
                    .Email(requestDTO.Email)
                    .Request(requestDTO)
                    .CreatedAt(requestDTO.ReceivedDate)
                    .TypeId(type)
                    .Build();
             
            return await _processedCasesRepository.Create(processedCase);
        }
    }
}
