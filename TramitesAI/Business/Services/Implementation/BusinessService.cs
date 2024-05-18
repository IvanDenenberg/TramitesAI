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
        private IRepositorio<SolicitudProcesada> _processedCasesRepository;
        private IAIHandler _AIHandler;
        private IFileSearcher _fileSearcher;

        public BusinessService(IRepositorio<SolicitudProcesada> processedCasesRepository, IAIHandler aIHandler, IFileSearcher fileSearcher)
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
                string requestId = await GuardarSolicitud(requestDTO);
                // Determine the type and validity
                string type = DetermineType(requestDTO);

                // Extract info from the request and save it in the database
                int id = await SaveNewCaseAsync(requestDTO, type);

                // Get files from external storage
                List<FileStream> files = GetFilesFromRequest(requestDTO.Attachments);

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

        private Task<string> GuardarSolicitud(RequestDTO requestDTO)
        {
            throw new NotImplementedException();
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

        private async void UpdateCase(ResponseDTO responseDTO, int id, string typeID)
        {
            SolicitudProcesada processedCasesDTO = await _processedCasesRepository.LeerPorId(id);
            {
                // Add info that was not available before analysis
                processedCasesDTO.Modificado = DateTime.Now;
                processedCasesDTO.TramiteId = typeID;
            }

            _ = _processedCasesRepository.Modificar(processedCasesDTO);
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

        private async Task<int> SaveNewCaseAsync(RequestDTO requestDTO, string type)
        {
            SolicitudProcesada processedCase = SolicitudProcesada.Builder()
                    .MsgId(requestDTO.MsgId)
                    .Canal(requestDTO.Channel)
                    .Email(requestDTO.Email)
                    .Creado(requestDTO.ReceivedDate)
                    .TipoTramite(type)
                    .Build();
             
            return await _processedCasesRepository.Crear(processedCase);
        }
    }
}
