using System.ComponentModel.DataAnnotations;

namespace TramitesAI.src.Repository.Domain.Entidades
{
    public class Solicitud
    {
        [Key]
        public int Id { get; set; }
        public string MensajeSolicitud { get; set; }

        private Solicitud() { }

        public static SolicitudBuilder Builder()
        {
            return new SolicitudBuilder();
        }

        public class SolicitudBuilder
        {
            private Solicitud dto = new Solicitud();

            public SolicitudBuilder Id(int id)
            {
                dto.Id = id;
                return this;
            }

            public SolicitudBuilder MensajeSolicitud(string mensaje)
            {
                dto.MensajeSolicitud = mensaje;
                return this;
            }

            public Solicitud Build()
            {
                return dto;
            }
        }
    }

}
