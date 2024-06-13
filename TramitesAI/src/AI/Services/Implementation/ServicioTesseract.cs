using System.Collections.Generic;
using System.IO;
using System.Text;
using Tesseract;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Common.Exceptions;

namespace TramitesAI.src.AI.Services.Implementation
{
    public class ServicioTesseract : IExtractorInformacion
    {
        public List<InformacionExtraidaDTO> extraerInformacionDeArchivos(List<MemoryStream> files)
        {
            List<InformacionExtraidaDTO> list = new List<InformacionExtraidaDTO>();

            foreach (MemoryStream file in files)
            {
                int position = 1;
                Console.WriteLine("Extracting info from file " + position + " of " + files.Count);
                InformacionExtraidaDTO extractedInfo = extractInfoFromFile(file);
                list.Add(extractedInfo);
                position++;
            }
            return list;

        }

        public InformacionExtraidaDTO extractInfoFromFile(MemoryStream file)
        {
            try
            {
                // Initialization of the Tesseract engine
                using (var engine = new TesseractEngine(@"./Tesseract/tessdata", "spa", EngineMode.Default))
                {
                    List<byte[]> imageData;
                    List<InformacionExtraidaDTO> parcialResult = new List<InformacionExtraidaDTO>();

                    if (EsPDF(file))
                    {
                        Console.WriteLine("Convirtiendo archivo PDF a PNG");
                        imageData = ConvertirPDFaPNG(file, engine);
                        Console.WriteLine("Convertido");
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
                                InformacionExtraidaDTO result = LeerInformacion(page);
                                parcialResult.Add(result);
                            }
                        }
                    }
                    Console.WriteLine("Informacion extraida");
                    return GenerarInformacionExtraidaDTO(parcialResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando archivo: {ex.Message}");
                throw new ApiException(ErrorCode.ERROR_EXTRAYENDO_TEXTOS );
            }

        }


        //Create and return the ExtractedInfoDTO object with the processed data.
        private InformacionExtraidaDTO GenerarInformacionExtraidaDTO(List<InformacionExtraidaDTO> resultadoParcial)
        {
            float confianzaMedia = 0;
            int cantidadElementos = resultadoParcial.Count;

            StringBuilder texto = new StringBuilder();

            foreach (InformacionExtraidaDTO resultado in resultadoParcial)
            {
                confianzaMedia += resultado.Confianza;
                texto.Append(resultado.Texto);
            }

            Console.WriteLine("InformacionExtraidaDTO creado");
            return InformacionExtraidaDTO.Builder()
                .Confianza(confianzaMedia / cantidadElementos)
                .Texto(texto.ToString())
                .Build();
        }

        private static bool EsPDF(MemoryStream archivo)
        {
            //Se leen los primeros 4 bytes que indican el formato del archivo
            byte[] header = new byte[4]; 
            archivo.Seek(0, SeekOrigin.Begin);
            archivo.Read(header, 0, header.Length);

            return header[0] == 0x25 && header[1] == 0x50 && header[2] == 0x44 && header[3] == 0x46;
        }

        static List<byte[]> ConvertirPDFaPNG(MemoryStream archivo, TesseractEngine engine)
        {
            try
            {
                byte[] pdfFileAsByte = archivo.ToArray();

                // Convert the PDF to PNG
                List<byte[]> pngFilesAsBytes = Freeware.Pdf2Png.ConvertAllPages(pdfFileAsByte);
                return pngFilesAsBytes;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error convirtiendo de PDF a PNG: {ex.Message}");
                throw;
            }
        }


        // GetInformation is a method that once the information is extracted by tesseract gets the confidence and the text of each analysis
        static InformacionExtraidaDTO LeerInformacion(Page pagina)
        {
            InformacionExtraidaDTO infoDTO = new InformacionExtraidaDTO();
            var texto = pagina.GetText();
            bool contieneLetras = texto.Any(char.IsLetter);
            if (contieneLetras)
            {
                infoDTO.Texto = texto;
                infoDTO.Confianza = pagina.GetMeanConfidence();
            }
            else
            {
                Console.WriteLine("Imagen ilustrativa");
            }
            return infoDTO;
        }

    }
}
