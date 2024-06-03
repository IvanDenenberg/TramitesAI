namespace TramitesAI.src.Repository.Domain.Entidades
{
    public class TramiteDato
    {
        public int TramiteId { get; set; }
        public Tramite Tramite { get; set; }

        public int DatoId { get; set; }
        public Dato Dato { get; set; }
    }
}