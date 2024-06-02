namespace TramitesAI.Repository.Domain.Dto
{
    public class TramiteDato
    {
        public int TramiteId { get; set; }
        public Tramite Tramite { get; set; }

        public int DatoId { get; set; }
        public Dato Dato { get; set; }
    }
}