using TramitesAI.AI.Services.Implementation;
using TramitesAI.AI.Services.Interfaces;
using TramitesAI.Business.Services.Implementation;
using TramitesAI.Business.Services.Interfaces;
using TramitesAI.Repository.Domain.Dto;
using TramitesAI.Repository.Implementations;
using TramitesAI.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBusinessService, BusinessService>();
builder.Services.AddSingleton<IAIHandler, AIHandler>();
builder.Services.AddSingleton<IAIInformationExtractor, TesseractService>();
builder.Services.AddSingleton<IAIAnalyzer, TensorFlowService>();
builder.Services.AddSingleton<IRepository<ProcessedCasesDTO>, ProcessedCasesRepository>();
builder.Services.AddSingleton<IFileSearcher, GoogleDriveSearcherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();