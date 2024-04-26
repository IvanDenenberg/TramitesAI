using TramitesAI.Business.Services.Interfaces;
using TramitesAI.Common.Exceptions;

namespace TramitesAI.Business.Services.Implementation
{
    public class GoogleDriveSearcherService : IFileSearcher
    {
        public FileStream GetFile(string path)
        {
            // Initialize HttpClient
            using var httpClient = new HttpClient();
            try
            {
                // Send request to the URL 
                HttpResponseMessage response = httpClient.GetAsync(path).Result;
                response.EnsureSuccessStatusCode();

                using FileStream files = (FileStream) response.Content.ReadAsStreamAsync().Result;
                
                return files;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
                throw new ApiException(ErrorCode.ERROR_DOWNLOAD_FILE);
            }
        }
    }
}
