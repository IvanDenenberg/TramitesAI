namespace TramitesAI.Business.Services.Interfaces
{
    public interface IFileSearcher
    {
        public FileStream GetFile(string path);
    }
}
