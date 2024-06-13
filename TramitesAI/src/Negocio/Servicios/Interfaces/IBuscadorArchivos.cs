namespace TramitesAI.src.Business.Services.Interfaces
{
    public interface IBuscadorArchivos


    {
        public MemoryStream ObtenerArchivo(string nombreArchivo, string msgId);
    }
}
