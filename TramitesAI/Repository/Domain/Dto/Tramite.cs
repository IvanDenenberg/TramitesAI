namespace TramitesAI.Repository.Domain.Dto
{
    public class Tramite
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<TramiteArchivo> TramiteArchivos { get; set; }
        public ICollection<TramiteDato> TramiteDatos { get; set; }
    }
}
