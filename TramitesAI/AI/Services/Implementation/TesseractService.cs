using System.Collections.Generic;
using System.IO;
using System.Text;
using Tesseract;
using TramitesAI.AI.Domain.Dto;
using TramitesAI.AI.Services.Interfaces;
using TramitesAI.Common.Exceptions;

namespace TramitesAI.AI.Services.Implementation
{
    public class TesseractService : IAIInformationExtractor
    {
        public List<ExtractedInfoDTO> extractInfoFromFiles(List<MemoryStream> files)
        {
            List<ExtractedInfoDTO> list = new List<ExtractedInfoDTO>();

            foreach (MemoryStream file in files)
            {
                int position = 1;
                Console.WriteLine("Extracting info from file " + position + " of " + files.Count);
                ExtractedInfoDTO extractedInfo = extractInfoFromFile(file);
                list.Add(extractedInfo);
                position++;
            }
            return list;

        }

        public ExtractedInfoDTO extractInfoFromFile(MemoryStream file)
        {
            try
            {
                // Initialization of the Tesseract engine
                using (var engine = new TesseractEngine(@"./Tesseract/tessdata", "spa", EngineMode.Default))
                {
                    List<byte[]> imageData;
                    List<ExtractedInfoDTO> parcialResult = new List<ExtractedInfoDTO>();

                    if (IsPDF(file))
                    {
                        Console.WriteLine("Its a PDF file, parsing to PNG");
                        imageData = ConvertPdfToPng(file, engine);
                        Console.WriteLine("Parsed");
                    }
                    else
                    {
                        imageData = new List<byte[]>{
                            file.ToArray()
                        };
                    }

                    foreach (var data in imageData) 
                    {
                        using (var img = Pix.LoadFromMemory(data))
                        {
                            using (var page = engine.Process(img))
                            {
                                ExtractedInfoDTO result = GetInformation(page);
                                parcialResult.Add(result);
                            }
                        }
                    }
                    Console.WriteLine("Information extracted, creating DTO");
                    return CreateFinalResult(parcialResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing file: {ex.Message}");
                throw new ApiException(ErrorCode.FAIL_PARSING_FILE);
            }

        }


        //Create and return the ExtractedInfoDTO object with the processed data.
        private ExtractedInfoDTO CreateFinalResult(List<ExtractedInfoDTO> parcialResult)
        {
            float meanConfidence = 0;
            int elementCount = parcialResult.Count;

            StringBuilder text = new StringBuilder();

            foreach (ExtractedInfoDTO result in parcialResult)
            {
                meanConfidence +=  result.Confidence;
                text.Append(result.Text);
            }

            Console.WriteLine("ExtractedInfoDTO created");
            return ExtractedInfoDTO.Builder()
                .Confidence(meanConfidence / elementCount)
                .Text(text.ToString())
                .Build();
        }

        private static bool IsPDF(MemoryStream file)
        {
            byte[] header = new byte[4]; // Adjust the array size according to the file's magic signature (%PDF)
            file.Seek(0, SeekOrigin.Begin);
            file.Read(header, 0, header.Length);

            return header[0] == 0x25 && header[1] == 0x50 && header[2] == 0x44 && header[3] == 0x46;
        }

        static List<byte[]> ConvertPdfToPng(MemoryStream file, TesseractEngine engine)
        {
            try
            {
                byte[] pdfFileAsByte = file.ToArray();

                // Convert the PDF to PNG
                List<byte[]> pngFilesAsBytes = Freeware.Pdf2Png.ConvertAllPages(pdfFileAsByte);
                return pngFilesAsBytes;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception was thrown during the PDF to PNG conversion: {ex.Message}");
                throw;

            }
        }


        // GetInformation is a method that once the information is extracted by tesseract gets the confidence and the text of each analysis
        static ExtractedInfoDTO GetInformation(Page page)
        {
            ExtractedInfoDTO infoDTO = new ExtractedInfoDTO();
            var text = page.GetText();
            bool hasLetters = text.Any(char.IsLetter);
            if (hasLetters)
            {
                infoDTO.Text = text;
                infoDTO.Confidence = page.GetMeanConfidence();
            }
            else
            {
                Console.WriteLine("Imagen ilustrativa");
            }
            return infoDTO;
        }

    }
}
