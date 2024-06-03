using System.Text.Json;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Business.Services.Interfaces;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repository.Domain.Entidades;
using TramitesAI.src.Repository.Interfaces;

namespace TramitesAI.src.Business.Services.Implementation
{
    public class BusinessService : IBusinessService
    {
        private IRepositorio<SolicitudProcesada> _solicitudProcesadaRepositorio;
        private IAIHandler _AIHandler;
        private IFileSearcher _fileSearcher;
        private IRepositorio<Solicitud> _solicitudRepositorio;

        public BusinessService(IRepositorio<SolicitudProcesada> solicitudProcesadaRepositorio, IAIHandler aIHandler, IFileSearcher fileSearcher, IRepositorio<Solicitud> solicitudRepositorio)
        {
            _solicitudProcesadaRepositorio = solicitudProcesadaRepositorio;
            _AIHandler = aIHandler;
            _fileSearcher = fileSearcher;
            _solicitudRepositorio = solicitudRepositorio;
        }

        public ResponseDTO GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> ProcessAsync(SolicitudDTO solicitudDTO)
        {
            try
            {
                Solicitud solicitud = await GuardarSolicitud(solicitudDTO);
                // Determine the type and validity
                int tipo = DeterminarTipo(solicitudDTO);

                // Extract info from the request and save it in the database
                int id = await GuardarSolicitudProcesada(solicitudDTO, tipo, solicitud);

                // Get files from external storage
                List<MemoryStream> files = GetFilesFromRequest(solicitudDTO.Attachments, solicitudDTO.MsgId);
                // Process Case
                // Extract info from attachments and analyze
                AnalyzedInformationDTO analyzedInformation = _AIHandler.ProcessInfo(files, solicitudDTO);

                //Generate Response
                ResponseDTO responseDTO = GenerateResponse(analyzedInformation);

                // Update with response
                UpdateCase(responseDTO, id, tipo);

                return new ResponseDTO();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<Solicitud> GuardarSolicitud(SolicitudDTO solicitudDto)
        {
            string solicitudString = JsonSerializer.Serialize(solicitudDto);
            Solicitud solicitudAGuardar = Solicitud.Builder()
                .MensajeSolicitud(solicitudString)
                .Build();

            int id = await _solicitudRepositorio.Crear(solicitudAGuardar);
            solicitudAGuardar.Id = id;

            return solicitudAGuardar;
        }

        private int DeterminarTipo(SolicitudDTO requestDTO)
        {
            // Return id_tramite
            return _AIHandler.DetermineType(requestDTO);
        }

        private ResponseDTO GenerateResponse(AnalyzedInformationDTO analyzedInformation)
        {
            throw new ApiException(ErrorCode.UNKNOWN_ERROR);
        }

        private async void UpdateCase(ResponseDTO responseDTO, int id, int typeID)
        {
            SolicitudProcesada processedCasesDTO = await _solicitudProcesadaRepositorio.LeerPorId(id);
            {
                // Add info that was not available before analysis
                processedCasesDTO.Modificado = DateTime.Now;
                processedCasesDTO.TramiteId = typeID;
            }

            _ = _solicitudProcesadaRepositorio.Modificar(processedCasesDTO);
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

        private async Task<int> GuardarSolicitudProcesada(SolicitudDTO solicitudDTO, int tipo, Solicitud solicitud)
        {
            SolicitudProcesada solicitudProcesada = SolicitudProcesada.Builder()
                    .MsgId(solicitudDTO.MsgId)
                    .Canal(solicitudDTO.Channel)
                    .Email(solicitudDTO.Email)
                    .Creado(solicitudDTO.ReceivedDate)
                    .TipoTramite(tipo)
                    .Solicitud(solicitud)
                    .Build();

            int id = await _solicitudProcesadaRepositorio.Crear(solicitudProcesada);
            solicitudProcesada.Id = id;

            ActualizarSolicitud(solicitudProcesada, solicitud.Id);

            return id;

        }

        private void ActualizarSolicitud(SolicitudProcesada solicitudProcesada, int idSolicitud)
        {
            throw new NotImplementedException();
        }
    }
}
