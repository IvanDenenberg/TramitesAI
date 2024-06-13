using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using TramitesAI.src.AI.Services.Implementation;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Services.Implementation;
using TramitesAI.src.Business.Services.Interfaces;
using TramitesAI.src.Repository.Configuration;
using TramitesAI.src.Repository.Domain.Entidades;
using TramitesAI.src.Repository.Implementations;
using TramitesAI.src.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tramites AI C# API",
        Description = "API para procesar solicitudes de tramites para la empresa BDT",
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Servicios config
builder.Services.AddSingleton<IServicioNegocio, BusinessService>();
builder.Services.AddSingleton<IAIHandler, AIHandler>();
builder.Services.AddSingleton<IExtractorInformacion, ServicioTesseract>();
builder.Services.AddSingleton<IAnalizadorAI, ProcesadorPython>();
builder.Services.AddSingleton<IBuscadorArchivos, GoogleDriveSearcherService>();

// Base de datos config
string server = Environment.GetEnvironmentVariable("SERVER_NAME");
string connectionString = "DatabaseConnection";
connectionString += server;
Console.WriteLine(connectionString);

builder.Services.AddDbContext<ConfigDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(connectionString)),
    ServiceLifetime.Singleton);

// Repositorios config
builder.Services.AddSingleton<IRepositorio<Archivo>, ArchivoRepositorio>();
builder.Services.AddSingleton<IRepositorio<Dato>, DatoRepositorio>();
builder.Services.AddSingleton<IRepositorio<Respuesta>, RespuestaRepositorio>();
builder.Services.AddSingleton<IRepositorio<SolicitudProcesada>, SolicitudProcesadaRepositorio>();
builder.Services.AddSingleton<IRepositorio<Solicitud>, SolicitudRepositorio>();
builder.Services.AddSingleton<IRepositorio<Tramite>, TramiteRepositorio>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();