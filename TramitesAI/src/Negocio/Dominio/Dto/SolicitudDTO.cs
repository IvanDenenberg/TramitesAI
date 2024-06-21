namespace TramitesAI.src.Business.Domain.Dto
{
    public class SolicitudDTO
    {
        public string MsgId { get; set; }
        public string Canal { get; set; }
        public string Origen { get; set; }
        public DateTime Recibido { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public List<string> Adjuntos { get; set; }

    }
}
