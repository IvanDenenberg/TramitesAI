namespace TramitesAI.src.Repository.Domain.Entidades
{
    public class Tramite
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<TramiteArchivo> TramiteArchivos { get; set; }
        public ICollection<TramiteDato> TramiteDatos { get; set; }
    }
}
