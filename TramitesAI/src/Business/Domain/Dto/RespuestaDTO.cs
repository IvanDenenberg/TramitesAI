using TramitesAI.src.Repository.Domain.Entidades;

namespace TramitesAI.src.Business.Domain.Dto
{
    public class RespuestaDTO
    {
        public string mensaje { get; set; }
        public bool valido { get; set; }
        public Dictionary<string, object> datosEncontrados {  get; set; }
        public List<string> datosFaltantes { get; set; }

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
                dto.mensaje = mensaje;
                return this;
            }

            public RespuestaDTOBuilder Valido(bool valido)
            {
                dto.valido = valido;
                return this;
            }

            public RespuestaDTOBuilder DatosEncontrados(Dictionary<string, object> datosEncontrados)
            {
                dto.datosEncontrados = datosEncontrados;
                return this;
            }

            public RespuestaDTOBuilder DatosFaltantes(List<string> datosFaltantes)
            {
                dto.datosFaltantes = datosFaltantes;
                return this;
            }

            public RespuestaDTO Build()
            {
                return dto;
            }
        }
    }
}
