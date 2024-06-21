using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tesseract;
using TramitesAI.src.Comun.Servicios.Implementaciones;
using TramitesAI.src.Comun.Servicios.Interfaces;
using TramitesAI.src.Repository.Configuration;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly Mock<IHttpClientWrapper> _mockHttpClientWrapper;

    public CustomWebApplicationFactory()
    {
        _mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {

            // Configuración común
            services.AddControllers();

            var descriptor = services.SingleOrDefault(
                 d => d.ServiceType == typeof(IHttpClientWrapper));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Configuración específica para pruebas
            services.AddSingleton<IHttpClientWrapper>(_mockHttpClientWrapper.Object);

            // Configuración específica para pruebas
            services.AddSingleton(provider =>
            {
                var tessDataPath = "C:\\Users\\ivand\\source\\repos\\TramitesAI\\TramitesAITest\\Tesseract\\tessdata\\";
                var language = "spa";
                var engineMode = EngineMode.Default;
                return new TesseractEngine(tessDataPath, language, engineMode);
            });

            // Otros servicios según necesidades de pruebas
            // Eliminar la configuración existente de la base de datos
            var descriptorBD = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ConfigDBContext>));

            if (descriptorBD != null)
            {
                services.Remove(descriptorBD);
            }

            // Crear las opciones para el DbContext
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            services.AddDbContext<ConfigDBContext>(options =>
                options.UseInMemoryDatabase("FunctionalTestDB"),
                ServiceLifetime.Singleton);

            // Crear un ámbito para aplicar las migraciones a la base de datos en memoria
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ConfigDBContext>();
                db.Database.EnsureCreated();
            }
        });
    }
}
