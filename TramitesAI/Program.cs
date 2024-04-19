using Tesseract;
using IronPdf;
using IronOcr;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

// Main method to perform OCR
static void PerformOCR()
{
    try
    {
        // Inicialización del motor de Tesseract
        using (var engine = new TesseractEngine(@"./tessdata", "spa", EngineMode.Default))
        {
            // OK: otros_ejemplos2.png - 12346-imagen00231.jpg - 12345-imagen00231.jpeg - 12345-foto1.jpg
            // Failed - otros_ejemplos1.pdf - 12347-imagen00231.docx
            string filePath = "./images/otros_ejemplos1.pdf";
            // Evaluamos si es un archivo pdf. Seguro haya que cambiar esta lógica en base a cómo recibimos el archivo
            bool terminaEnPDF = filePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase); 

            if (terminaEnPDF)
            {
                Console.WriteLine("El archivo termina en .pdf");
            }
            else
            {
                  // Acceso al archivo
                  using (var img = Pix.LoadFromFile(filePath)) 
                  {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            //Para verificar que no sea una imagen ilustrativa evaluamos que text no contenga texto
                            bool contieneLetras = text.Any(char.IsLetter);

                            if (contieneLetras)
                            {
                                Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence()); //CLIENTES: Ya desde este dato filtramos respuesta?
                                Console.WriteLine("Text (GetText): {0}", text); // Obtenemos el texto estraido por el OCR
                            }
                            else
                            {
                                Console.WriteLine("Imagen ilustrativa");
                            }

                            //Console.WriteLine("Text (iterator):");
                        }
                  }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Se lanzó una excepción: {ex.Message}");
    }

}

PerformOCR();

app.Run();
