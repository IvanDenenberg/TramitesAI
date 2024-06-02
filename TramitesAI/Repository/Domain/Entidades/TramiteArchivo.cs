namespace TramitesAI.Repository.Domain.Dto
{
    public class TramiteArchivo
    {
        public int TramiteId { get; set; }
        public Tramite Tramite { get; set; }

        public int ArchivoId { get; set; }
        public Archivo Archivo { get; set; }
    }
}