namespace TramitesAI.src.Repository.Domain.Entidades
{
    public class Dato
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<TramiteDato> TramiteDatos { get; set; }
    }
}
