using AdasIt.Andor.Configurations.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.UseConfigurations(builder.Configuration);

var app = builder.Build();

app.MapControllers();

app.Run();