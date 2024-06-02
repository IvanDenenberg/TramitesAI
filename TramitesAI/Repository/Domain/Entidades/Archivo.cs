namespace TramitesAI.Repository.Domain.Dto
{
    public class Archivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Obligatorio { get; set; }
        public ICollection<TramiteArchivo> TramiteArchivos { get; set; }
    }
}
