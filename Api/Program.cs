using Api.Filters;
using Api.Startup;
using Api.Startup.Swagger;
using Core.Settings;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.SetupSettings();

services.AddSingleton(builder.Configuration.Get<AppSecrets>());
services.AddSingleton(builder.Configuration.Get<AppSettings>());

services.AddControllers();
services.AddHttpClient();
services.AddDependencies();
services.AddSwagger();

services.AddMvc(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilter));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();