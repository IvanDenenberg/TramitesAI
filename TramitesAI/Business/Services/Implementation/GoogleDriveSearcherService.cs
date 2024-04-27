using TramitesAI.Business.Services.Interfaces;
using TramitesAI.Common.Exceptions;

namespace TramitesAI.Business.Services.Implementation
{
    public class GoogleDriveSearcherService : IFileSearcher
    {
        public Stream GetFile(string path)
        {
            // Initialize HttpClient
            using var httpClient = new HttpClient();
            try
            {
                // Send request to the URL 
                HttpResponseMessage response = httpClient.GetAsync(path).Result;
                response.EnsureSuccessStatusCode();

                using Stream files = response.Content.ReadAsStreamAsync().Result;

                using (FileStream fileStream = File.Create("C:/Users/ivand/Downloads/ORT Proyectos 2024/Archivos de prueba/prueba1.txt"))
                {
                    files.CopyTo(fileStream);
                }

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
