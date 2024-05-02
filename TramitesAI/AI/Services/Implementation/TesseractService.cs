using System.Collections.Generic;
using System.IO;
using Tesseract;
using TramitesAI.AI.Domain.Dto;
using TramitesAI.AI.Services.Interfaces;

namespace TramitesAI.AI.Services.Implementation
{
    public class TesseractService : IAIInformationExtractor
    {
        public List<ExtractedInfoDTO> extractInfoFromFiles(List<Stream> files)
        {
            List < ExtractedInfoDTO > extractedInfoList = new List < ExtractedInfoDTO >();

            //foreach (FileStream file in files)
            //{
            //    ExtractedInfoDTO extractedInfo = extractInfoFromFile(file);
            //    extractedInfoList.Add(extractedInfo);
            //}

            return extractedInfoList;

        }

        private ExtractedInfoDTO extractInfoFromFile(MemoryStream file)
        {
            try
            {
                // Inicialización del motor de Tesseract
                using (var engine = new TesseractEngine(@"./tessdata", "spa", EngineMode.Default))
                {
                    // Verificar si el archivo es un PDF (por ejemplo, mediante la extensión del nombre de archivo)
                    //bool esPDF = file is FileStream && ((FileStream)file).Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);

                    //List<string> pngFilePaths = new List<string>();

                    //if (false)
                    //{
                    //    // Convertir el PDF a PNG y obtener las rutas de archivo de las imágenes PNG generadas
                    //    //pngFilePaths = ConvertirPDFAPNG(file, engine);
                    //    ConvertirPDFAPNG(file, engine, pngFilePaths);

                    //}
                    //else
                    //{
                    //    string tempFilePath = Path.GetTempFileName();
                    //    using (var tempFileStream = File.OpenWrite(tempFilePath))
                    //    {
                    //        file.CopyTo(tempFileStream);
                    //    }
                    //    pngFilePaths.Add(tempFilePath);
                    //}

                    //// Iterar sobre las rutas de archivo de las imágenes PNG generadas o del Stream original
                    //int i = 1;
                    //foreach (var pngFilePath in pngFilePaths)
                    //{
                    //    ProcesarArchivo(pngFilePath, engine);
                    //    Console.WriteLine("Página " + i);
                    //    i++;
                    //}
                    ProcesarArchivo(file, engine);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se lanzó una excepción: {ex.Message}");
            }
            return new ExtractedInfoDTO();
        }

        static void ConvertirPDFAPNG(Stream fileStream, TesseractEngine engine, List<string> pngFilePaths)
        {
            //List<string> pngFilePaths = new List<string>();

            try
            {
                byte[] pdfFileAsByte;

                // Verificar si el Stream es un FileStream y leer los datos del archivo
                if (fileStream is FileStream)
                {
                    pdfFileAsByte = File.ReadAllBytes(((FileStream)fileStream).Name);
                }
                else
                {
                    // El Stream no es un FileStream, leer los datos del Stream
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        pdfFileAsByte = memoryStream.ToArray();
                    }
                }

                // Convertir el PDF a PNG
                List<byte[]> pngFilesAsBytes = Freeware.Pdf2Png.ConvertAllPages(pdfFileAsByte);

                string pdfFileNameWithoutExtension = Path.GetFileNameWithoutExtension(((FileStream)fileStream).Name);
                int pageCounter = 0;

                // Guardar cada página convertida como un archivo PNG y agregar la ruta al listado
                foreach (byte[] onePngAsBytes in pngFilesAsBytes)
                {
                    pageCounter++;
                    string pngFilePath = Path.Combine(@"./images/PdfsToPng/", $"{pdfFileNameWithoutExtension}_{pageCounter}.png");
                    File.WriteAllBytes(pngFilePath, onePngAsBytes);
                    pngFilePaths.Add(pngFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se lanzó una excepción durante la conversión de PDF a PNG: {ex.Message}");
            }

            //return pngFilePaths;
        }


        static void ProcesarArchivo(MemoryStream file, TesseractEngine engine)
        {
            //byte[] imageData = File.ReadAllBytes(file);
            byte[] imageData = file.ToArray();

            using (var img = Pix.LoadFromMemory(imageData))
            {
                using (var page = engine.Process(img))
                {
                    var text = page.GetText();
                    bool contieneLetras = text.Any(char.IsLetter);
                    if (contieneLetras)
                    {
                        Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());
                        Console.WriteLine("Text (GetText): {0}", text);
                    }
                    else
                    {
                        Console.WriteLine("Imagen ilustrativa");
                    }
                }
            }
        }
    }
}
