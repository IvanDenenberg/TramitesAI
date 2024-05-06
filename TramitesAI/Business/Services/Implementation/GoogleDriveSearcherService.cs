using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Download;
using TramitesAI.Business.Services.Interfaces;
using TramitesAI.Common.Exceptions;
using Google.Apis.Drive.v3.Data;

namespace TramitesAI.Business.Services.Implementation
{
    public class GoogleDriveSearcherService : IFileSearcher
    {
        public GoogleDriveSearcherService() { }
        public MemoryStream GetFile(string fileName, string msgId)
        {
            try
            {
                // Get JsonApiKey for Google Drive and its validation
                string jsonApiKey = @"./Credentials/tramitesai-366f1be7dfc8.json";

                if (jsonApiKey == null)
                {
                    throw new ApiException(ErrorCode.MISSING_CONFIG_PROPERTY);
                }
                Console.WriteLine("Api key found");

                // Connecting to Google Drive
                GoogleCredential credential = GoogleCredential.FromFile(jsonApiKey)
                    .CreateScoped(new[] { DriveService.ScopeConstants.Drive });

                DriveService service = new(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });

                Console.WriteLine("Connected to google drive");

                // Defining target file name
                String targetFileName = $"{msgId}_{fileName}";
                
                // Searching by file name to obtain id and then downloading by id
                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.Q = $"name='{targetFileName}'";

                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
                var stream = new MemoryStream();
                var targetFile = files.FirstOrDefault(file => file.Name == targetFileName);

                if (targetFile != null)
                {
                    Console.WriteLine("File found");
                    var request = service.Files.Get(targetFile.Id);
                    request.Download(stream);
                    stream.Position = 0;
                    Console.WriteLine("File downloaded");
                } else
                {
                    throw new ApiException(ErrorCode.FILE_NOT_FOUND);
                }

                return stream;
            }
            catch (Exception e)
            {
                if (e is ApiException exception)
                {
                    Console.WriteLine($"Error: {exception.Message}");
                    throw exception;
                } else
                {
                    Console.WriteLine($"Error: {e.Message}");
                    throw new ApiException(ErrorCode.ERROR_DOWNLOAD_FILE);
                }
            }
        }
    }
}
