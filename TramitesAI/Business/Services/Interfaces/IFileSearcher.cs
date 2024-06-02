namespace TramitesAI.Business.Services.Interfaces
{
    public interface IFileSearcher
    {
        public MemoryStream GetFile(string fileName, string msgId);
    }
}
