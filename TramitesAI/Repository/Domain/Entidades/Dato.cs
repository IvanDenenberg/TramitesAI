namespace TramitesAI.Repository.Domain.Dto
{
    public class Dato
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<TramiteDato> TramiteDatos { get; set; }
    }
}
