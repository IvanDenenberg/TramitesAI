using System.ComponentModel.DataAnnotations;

namespace TramitesAI.src.Repository.Domain.Entidades
{
    public class SolicitudProcesada
    {
        [Key]
        public int Id { get; set; }
        public string MsgId { get; private set; }
        public string Canal { get; private set; }
        public string Email { get; private set; }
        public int? TramiteId { get; set; }
        public DateTime Creado { get; private set; }
        public DateTime Modificado { get; set; }
        public int SolicitudId { get; set; }
        public Solicitud Solicitud { get; set; }
        public int? RespuestaId { get; set; }
        public Respuesta? Respuesta { get; set; }

        private SolicitudProcesada() { }

        public static SolicitudProcesadaBuilder Builder()
        {
            return new SolicitudProcesadaBuilder();
        }

        public class SolicitudProcesadaBuilder
        {
            private SolicitudProcesada dto = new SolicitudProcesada();

            public SolicitudProcesadaBuilder Id(int id)
            {
                dto.Id = id;
                return this;
            }

            public SolicitudProcesadaBuilder MsgId(string msgId)
            {
                dto.MsgId = msgId;
                return this;
            }

            public SolicitudProcesadaBuilder Canal(string canal)
            {
                dto.Canal = canal;
                return this;
            }

            public SolicitudProcesadaBuilder Email(string email)
            {
                dto.Email = email;
                return this;
            }

            public SolicitudProcesadaBuilder Creado(DateTime fecha)
            {
                dto.Creado = fecha;
                return this;
            }


            public SolicitudProcesadaBuilder TipoTramite(int tipo)
            {
                dto.TramiteId = tipo;
                return this;
            }


            public SolicitudProcesadaBuilder Solicitud(Solicitud solicitud)
            {
                dto.Solicitud = solicitud;
                return this;
            }

            public SolicitudProcesada Build()
            {
                return dto;
            }
        }
    }
}
