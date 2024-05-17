﻿using System.ComponentModel.DataAnnotations;

namespace TramitesAI.Repository.Domain.Dto
{
    public class Respuesta
    {
        [Key]
        public int Id { get; set; }
        public string MensajeRespuesta { get; set; }
        public SolicitudProcesada SolicitudProcesada { get; set; }

        private Respuesta() { }

        public static RespuestaBuilder Builder()
        {
            return new RespuestaBuilder();
        }

        public class RespuestaBuilder
        {
            private Respuesta dto = new Respuesta();

            public RespuestaBuilder Id(int id)
            {
                dto.Id = id;
                return this;
            }

            public RespuestaBuilder MensajeRespuesta(string mensajeRespuesta)
            {
                dto.MensajeRespuesta = mensajeRespuesta;
                return this;
            }

            public Respuesta Build()
            {
                return dto;
            }
        }
    }

}
