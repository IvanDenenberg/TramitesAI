using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using TramitesAI.src.Business.Services.Interfaces;
using TramitesAI.src.Common.Exceptions;

namespace TramitesAI.src.Business.Services.Implementation
{
    public class GoogleDriveSearcherService : IFileSearcher
    {
        public GoogleDriveSearcherService() { }
        public MemoryStream ObtenerArchivo(string nombreArchivo, string msgId)
        {
            try
            {
                // Obteniendo la Api Key para utilizar Google Drive y validandola
                string jsonApiKey = @"./Credentials/tramitesai-366f1be7dfc8.json";

                if (jsonApiKey == null)
                {
                    throw new ApiException(ErrorCode.MISSING_CONFIG_PROPERTY);
                }
                Console.WriteLine("Api key encontrada");

                // Conectando a Google Drive
                GoogleCredential credencial = GoogleCredential.FromFile(jsonApiKey)
                    .CreateScoped(new[] { DriveService.ScopeConstants.Drive });

                DriveService service = new(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credencial
                });

                Console.WriteLine("Conectado a google drive");

                // Definiendo el archivo a descargar
                string nombreArchivoBuscado = $"{msgId}_{nombreArchivo}";

                // Buscando dentro de Google Drive con el nombre del archivo para encontrar el ID
                // Una vez obtenido el ID se utiliza este para descargar al archivo
                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.Q = $"name='{nombreArchivoBuscado}'";

                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
                var stream = new MemoryStream();
                var archivoBuscado = files.FirstOrDefault(file => file.Name == nombreArchivoBuscado);

                if (archivoBuscado != null)
                {
                    Console.WriteLine("Archivo Encontrado");
                    var request = service.Files.Get(archivoBuscado.Id);
                    request.Download(stream);
                    stream.Position = 0;
                    Console.WriteLine("Archivo Descargado");
                }
                else
                {
                    throw new ApiException(ErrorCode.FILE_NOT_FOUND);
                }

                return stream;
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw ex;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw new ApiException(ErrorCode.ERROR_DOWNLOAD_FILE);
            }
        }
    }
}
