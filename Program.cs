using BAS_Project.Services;
using BAS_Project.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IFileProcessingService, FileProcessingService>();

var app = builder.Build();

app.MapControllers();

app.Run();
