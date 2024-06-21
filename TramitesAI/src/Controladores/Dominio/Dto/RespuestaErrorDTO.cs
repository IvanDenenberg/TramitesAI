namespace TramitesAI.src.Controladores.Dominio.Dto
{
    public class RespuestaErrorDTO
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        private RespuestaErrorDTO() { }

        public static RespuestaErrorDTOBuilder Builder()
        {
            return new RespuestaErrorDTOBuilder();
        }

        public class RespuestaErrorDTOBuilder
        {
            private RespuestaErrorDTO dto = new RespuestaErrorDTO();

            public RespuestaErrorDTOBuilder Codigo(string code)
            {
                dto.Codigo = code;
                return this;
            }

            public RespuestaErrorDTOBuilder Descripcion(string description)
            {
                dto.Descripcion = description;
                return this;
            }

            public RespuestaErrorDTO Build()
            {
                return dto;
            }
        }
    }
}
