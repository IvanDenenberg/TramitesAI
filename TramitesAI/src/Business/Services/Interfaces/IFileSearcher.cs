namespace TramitesAI.src.Business.Services.Interfaces
{
    public interface IFileSearcher
    {
        public MemoryStream ObtenerArchivo(string nombreArchivo, string msgId);
    }
}
