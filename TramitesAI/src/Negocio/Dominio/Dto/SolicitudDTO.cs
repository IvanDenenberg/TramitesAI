namespace TramitesAI.src.Business.Domain.Dto
{
    public class SolicitudDTO
    {
        public string MsgId { get; set; }
        public string Channel { get; set; }
        public string Email { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public List<string> Attachments { get; set; }

    }
}
