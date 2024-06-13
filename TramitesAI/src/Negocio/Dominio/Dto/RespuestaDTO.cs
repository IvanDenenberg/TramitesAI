using Newtonsoft.Json;
using TramitesAI.src.Repository.Domain.Entidades;

namespace TramitesAI.src.Business.Domain.Dto
{
    public class RespuestaDTO
    {
        public string Mensaje { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Valido { get; set; }
        public Dictionary<string, object> DatosEncontrados {  get; set; }
        public List<string> DatosFaltantes { get; set; }

        public int SolicitudProcesadaId { get; set; } 

        private RespuestaDTO() { }

        public static RespuestaDTOBuilder Builder()
        {
            return new RespuestaDTOBuilder();
        }

        public class RespuestaDTOBuilder
        {
            private RespuestaDTO dto = new RespuestaDTO();

            public RespuestaDTOBuilder Mensaje(string mensaje)
            {
                dto.Mensaje = mensaje;
                return this;
            }

            public RespuestaDTOBuilder Valido(bool valido)
            {
                dto.Valido = valido;
                return this;
            }

            public RespuestaDTOBuilder DatosEncontrados(Dictionary<string, object> datosEncontrados)
            {
                dto.DatosEncontrados = datosEncontrados;
                return this;
            }

            public RespuestaDTOBuilder DatosFaltantes(List<string> datosFaltantes)
            {
                dto.DatosFaltantes = datosFaltantes;
                return this;
            }

            public RespuestaDTOBuilder SolicitudProcesadaId(int solicitudProcesadId)
            {
                dto.SolicitudProcesadaId = solicitudProcesadId;
                return this;
            }

            public RespuestaDTO Build()
            {
                return dto;
            }
        }
    }
}
