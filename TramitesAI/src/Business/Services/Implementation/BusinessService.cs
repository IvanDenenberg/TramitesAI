using Microsoft.IdentityModel.Tokens;
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
        private readonly IRepositorio<SolicitudProcesada> _solicitudProcesadaRepositorio;
        private readonly IAIHandler _AIHandler;
        private readonly IFileSearcher _fileSearcher;
        private readonly IRepositorio<Solicitud> _solicitudRepositorio;
        private readonly IRepositorio<Tramite> _tramiteRepositorio;
        private readonly IRepositorio<Respuesta> _respuestaRepositorio;

        public BusinessService(IRepositorio<SolicitudProcesada> solicitudProcesadaRepositorio, IAIHandler aIHandler, IFileSearcher fileSearcher, IRepositorio<Solicitud> solicitudRepositorio, IRepositorio<Tramite> tramiteRepositorio, IRepositorio<Respuesta> respuestaRepositorio)
        {
            _solicitudProcesadaRepositorio = solicitudProcesadaRepositorio;
            _AIHandler = aIHandler;
            _fileSearcher = fileSearcher;
            _solicitudRepositorio = solicitudRepositorio;
            _tramiteRepositorio = tramiteRepositorio;
            _respuestaRepositorio = respuestaRepositorio;
        }

        public RespuestaDTO GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<RespuestaDTO> ProcessAsync(SolicitudDTO solicitudDTO)
        {
            try
            {
                Solicitud solicitud = await GuardarSolicitud(solicitudDTO);
                
                // Determinar el tipo de la solicitud
                Tramite tramite = await DeterminarTramite(solicitudDTO.Subject);
                Console.WriteLine("Tramite asociado");

                // Extraer la informacion, generar la SolicitudProcesada y guardarla en la DB
                int idSolicitudProcesada = await GuardarSolicitudProcesada(solicitudDTO, tramite.Id, solicitud);

                List<MemoryStream> archivos = new List<MemoryStream>();

                Console.WriteLine("Validando si el tramite requiere analizar los archivos");
                if (tramite.TramiteArchivos != null & tramite.TramiteArchivos.Count() > 0)
                {
                    // Obtener los archivos desde un almacenamiento externo
                    archivos = ObtenerArchivos(solicitudDTO.Attachments, solicitudDTO.MsgId);
                    Console.WriteLine("Archivos descargados");
                }

                // Procesar el tramite
                // Extraer el texto de los archivos y analizarlo
                InformacionAnalizadaDTO informacionAnalizada = await _AIHandler.ProcesarInformacion(archivos, solicitudDTO, tramite);
                Console.WriteLine("Informacion analizada");


                // Generar respuesta
                RespuestaDTO respuestaDTO = GenerarRespuesta(informacionAnalizada);

                Console.WriteLine("Respuesta generada");
                //Guardar la respuesta en la base de datos
                Respuesta respuesta = await GuardarRespuestaAsync(respuestaDTO);

                // Actualizar la solicitud procesada con la respuesta generada
                ActualizarSolicitudProcesada(respuesta, idSolicitudProcesada, tramite.Id);

                return respuestaDTO;
            }
            catch (ApiException ex)
            {
                if (ex.Code.Equals(ErrorCode.INVALID_SUBJECT.ToString()))
                {
                    return GenerarRespuestaTramiteInvalido();
                } else if (ex.Code.Equals(ErrorCode.MODEL_NOT_IMPLEMENTED.ToString()))
                {
                    return GenerarRespuestaModeloNoImplementado();
                } else
                {
                    throw ex;
                }
            }
            catch (Exception)
            {
                throw new ApiException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        private RespuestaDTO GenerarRespuestaModeloNoImplementado()
        {
            return RespuestaDTO.Builder()
                .Mensaje("El modelo para el tramite aun no fue implementado")
                .Build();
        }

        private async Task<Respuesta> GuardarRespuestaAsync(RespuestaDTO respuestaDTO)
        {
            string respuestaString = JsonSerializer.Serialize(respuestaDTO);
            Respuesta respuestaAGuardar = Respuesta.Builder()
                .MensajeRespuesta(respuestaString)
                .Build();

            int id = await _respuestaRepositorio.Crear(respuestaAGuardar);
            Console.WriteLine("Respuesta guardada");
            respuestaAGuardar.Id = id;

            return respuestaAGuardar;
        }

        private RespuestaDTO GenerarRespuestaTramiteInvalido()
        {
            return RespuestaDTO.Builder()
                .Mensaje("El tramite no es valido")
                .Valido(false) 
                .Build();
        }

        private async Task<Solicitud> GuardarSolicitud(SolicitudDTO solicitudDto)
        {
            string solicitudString = JsonSerializer.Serialize(solicitudDto);
            Solicitud solicitudAGuardar = Solicitud.Builder()
                .MensajeSolicitud(solicitudString)
                .Build();

            int id = await _solicitudRepositorio.Crear(solicitudAGuardar);
            Console.WriteLine("Solicitud guardada");
            solicitudAGuardar.Id = id;

            return solicitudAGuardar;
        }

        private async Task<Tramite> DeterminarTramite(string asunto)
        {
            try
            {
                // Determinar el ID utilizando el AIHandler
                int tramiteID = await _AIHandler.DeterminarTramiteAsync(asunto);

                return await _tramiteRepositorio.LeerPorId(tramiteID);
            } catch (ApiException ex)
            {
                if (ex.Code.Equals(ErrorCode.NOT_FOUND.ToString()))
                {
                    throw new ApiException(ErrorCode.INVALID_SUBJECT);
                } else
                {
                    throw;
                }
               
            } catch (Exception)
            {
                throw new ApiException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        private RespuestaDTO GenerarRespuesta(InformacionAnalizadaDTO informacionAnalizada)
        {
            List<string> datosFaltantes = new List<string>();
            Dictionary<string, object> datosEncontrados = new Dictionary<string, object>();
            bool valido = true;

            foreach (var campo in informacionAnalizada.Campos)
            {
                if (campo.Value == null) 
                {
                    datosFaltantes.Add(campo.Key);
                    valido = false;
                } else
                {
                    datosEncontrados.Add(campo.Key, campo.Value);
                }
            }

            RespuestaDTO respuesta = RespuestaDTO.Builder()
                .DatosEncontrados(datosEncontrados)
                .Valido(valido)
                .Build();

            if (!datosFaltantes.IsNullOrEmpty())
            {
                respuesta.datosFaltantes = datosFaltantes;
            }

            return respuesta;
        }

        private async void ActualizarSolicitudProcesada(Respuesta respuesta, int id, int typeID)
        {
            SolicitudProcesada solicitudProcesada = await _solicitudProcesadaRepositorio.LeerPorId(id);
            {
                // Agregando informacion que no estaba disponible antes
                solicitudProcesada.Modificado = DateTime.Now;
                solicitudProcesada.TramiteId = typeID;
                solicitudProcesada.RespuestaId = respuesta.Id;
                solicitudProcesada.Respuesta = respuesta;
            }

            _ = _solicitudProcesadaRepositorio.Modificar(solicitudProcesada);
            Console.WriteLine("Solicitud procesada actualizada");
        }

        private List<MemoryStream> ObtenerArchivos(List<string> adjuntos, string msgId)
        {
            int contador = 1;
            try
            {
                List<MemoryStream> archivos = new();
                foreach (string adjunto in adjuntos)
                {
                    MemoryStream archivoDescargado = _fileSearcher.ObtenerArchivo(adjunto, msgId);
                    archivos.Add(archivoDescargado);
                    Console.WriteLine("Archivo" + contador + " de " + adjuntos.Count() + " descargados");
                    contador++;
                }

                return archivos;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException(ErrorCode.INTERNAL_SERVER_ERROR);
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
                    .Solicitud(await _solicitudRepositorio.LeerPorId(solicitud.Id))
                    .Build();



            int id = await _solicitudProcesadaRepositorio.Crear(solicitudProcesada);
            Console.WriteLine("Solicitud procesada creada");

            return id;
        }
    }
}
