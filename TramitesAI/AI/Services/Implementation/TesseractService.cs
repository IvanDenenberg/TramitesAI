using System.Collections.Generic;
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

            foreach (FileStream file in files)
            {
                ExtractedInfoDTO extractedInfo = extractInfoFromFile(file);
                extractedInfoList.Add(extractedInfo);
            }

            return extractedInfoList;

        }

        private ExtractedInfoDTO extractInfoFromFile(Stream file)
        {
            try
            {
                // Inicialización del motor de Tesseract
                using (var engine = new TesseractEngine(@"./tessdata", "spa", EngineMode.Default))
                {
                    //string filePath = "./images/otros_ejemplos1.pdf";


                    // Verificar si el archivo es un PDF
                    bool esPDF = filePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);

                    List<string> pngFilePaths = new List<string>();

                    if (esPDF)
                    {
                        // Convertir el PDF a PNG
                        pngFilePaths = ConvertirPDFAPNG(filePath, engine);
                    }
                    else
                    {
                        // Agregara el archivo original
                        pngFilePaths.Add(filePath);
                    }

                    int i = 1;
                    foreach (var pngFilePath in pngFilePaths)
                    {
                        ProcesarArchivo(pngFilePath, engine);
                        Console.WriteLine("Pagina " + i);
                        i++;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se lanzó una excepción: {ex.Message}");
            }
            return new ExtractedInfoDTO();
        }

        static List<string> ConvertirPDFAPNG(string filePath, TesseractEngine engine)
        {
            List<string> pngFilePaths = new List<string>();

            string pdfFileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            byte[] pdfFileAsByte = File.ReadAllBytes(filePath);
            List<byte[]> pngFilesAsBytes = Freeware.Pdf2Png.ConvertAllPages(pdfFileAsByte);
            int pageCounter = 0;
            foreach (byte[] onePngAsBytes in pngFilesAsBytes)
            {
                pageCounter++;
                string pngFilePath = Path.Combine(@"./images/PdfsToPng/", $"{pdfFileNameWithoutExtension}_{pageCounter}.png");
                File.WriteAllBytes(pngFilePath, onePngAsBytes);
                pngFilePaths.Add(pngFilePath);
            }

            return pngFilePaths;
        }

        static void ProcesarArchivo(string filePath, TesseractEngine engine)
        {
            byte[] imageData = File.ReadAllBytes(filePath);

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
