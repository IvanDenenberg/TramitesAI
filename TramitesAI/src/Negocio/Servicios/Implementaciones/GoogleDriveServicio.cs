using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using System;
using TramitesAI.src.Business.Services.Interfaces;
using TramitesAI.src.Common.Exceptions;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TramitesAI.src.Business.Services.Implementation
{
    public class GoogleDriveServicio : IBuscadorArchivos
    {
        public DriveService DriveService { get; private set; }
        public GoogleDriveServicio(DriveService service) 
        {
            DriveService = service;
        }
        public MemoryStream ObtenerArchivo(string nombreArchivo, string msgId)
        {
            try
            {
                // Definiendo el archivo a descargar
                string nombreArchivoBuscado = $"{msgId}_{nombreArchivo}";

                // Buscando dentro de Google Drive con el nombre del archivo para encontrar el ID
                // Una vez obtenido el ID se utiliza este para descargar al archivo
                FilesResource.ListRequest listRequest = DriveService.Files.List();
                listRequest.Q = $"name='{nombreArchivoBuscado}'";

                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
                var stream = new MemoryStream();
                var archivoBuscado = files.FirstOrDefault(file => file.Name == nombreArchivoBuscado);

                if (archivoBuscado != null)
                {
                    Console.WriteLine("Archivo Encontrado");
                    var request = DriveService.Files.Get(archivoBuscado.Id);
                    request.Download(stream);
                    stream.Position = 0;
                    Console.WriteLine("Archivo Descargado");
                }
                else
                {
                    throw new ApiException(ErrorCode.ARCHIVO_NO_ENCONTRADO);
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
                throw new ApiException(ErrorCode.ERROR_DESCARGANDO_ARCHIVO);
            }
        }
    }
}
