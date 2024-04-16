using BAS_Project.Helpers;
using BAS_Project.Services;
using BAS_Project.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IFileProcessingService, FileProcessingService>();

builder.Services.Configure<MyConfigs>(builder.Configuration.GetSection("myConfigs"));

var app = builder.Build();

app.MapControllers();

app.Run();
