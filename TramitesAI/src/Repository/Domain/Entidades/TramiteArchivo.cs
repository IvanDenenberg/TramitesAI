namespace TramitesAI.src.Repository.Domain.Entidades
{
    public class TramiteArchivo
    {
        public int TramiteId { get; set; }
        public Tramite Tramite { get; set; }

        public int ArchivoId { get; set; }
        public Archivo Archivo { get; set; }
    }
}