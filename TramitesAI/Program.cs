using Microsoft.EntityFrameworkCore;
using TramitesAI.AI.Services.Implementation;
using TramitesAI.AI.Services.Interfaces;
using TramitesAI.Business.Services.Implementation;
using TramitesAI.Business.Services.Interfaces;
using TramitesAI.Repository.Configuration;
using TramitesAI.Repository.Domain.Dto;
using TramitesAI.Repository.Implementations;
using TramitesAI.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Services config
builder.Services.AddSingleton<IBusinessService, BusinessService>();
builder.Services.AddSingleton<IAIHandler, AIHandler>();
builder.Services.AddSingleton<IAIInformationExtractor, TesseractService>();
builder.Services.AddSingleton<IAIAnalyzer, TensorFlowService>();
builder.Services.AddSingleton<IFileSearcher, GoogleDriveSearcherService>();

// Database config
string server = Environment.GetEnvironmentVariable("SERVER_NAME");
string connectionString = "DatabaseConnection";
connectionString += server;
Console.WriteLine(connectionString);

builder.Services.AddDbContext<ConfigDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(connectionString))
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, LogLevel.Information),
    ServiceLifetime.Singleton);

// Repositories config
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
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();