using Api.Filters;
using Api.Startup;
using Api.Startup.Swagger;
using Core.Settings;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.SetupSettings();

services.AddSingleton(builder.Configuration.Get<AppSecrets>());
services.AddSingleton(builder.Configuration.Get<AppSettings>());

services.AddControllers();
services.AddDependencies();

services.AddMvc(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilter));
});

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Dream Reel API",
        Description = @"

API for Dream Reel.

<strong>Example Users:</strong>

ca0a33aa-c1fe-42b6-8ca7-bf78967375d5 => TestUsername1 + TestPassword1

56dad1f6-74f6-4550-997d-5ba1aef3c2c3 => TestUsername2 + TestPassword2$

3c4cd369-a1ed-45c0-ae8d-12490158f1b8 => TestUsername3 + TestPassword3$

<strong>Usage:</strong>

1. Log in with user

2. View reel with `/api/dreams/reel` endpoint (must be logged in)

3. Follow another user with `/api/users/follow/:userReference` (must be logged in)

4. Create dream with `/api/dreams` (must be logged in)

"
    });

    options.CustomSchemaIds(x => x.FullName?.Replace("+", "."));
    options.SchemaFilter<SwaggerEnumDescriptionsFilter>();
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
