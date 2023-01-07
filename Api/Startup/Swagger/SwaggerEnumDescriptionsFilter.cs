using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Startup.Swagger;

public class SwaggerEnumDescriptionsFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
            return;

        model.Enum.Clear();

        Enum.GetNames(context.Type)
            .ToList()
            .ForEach(x => model.Enum.Add(new OpenApiString($"{(int)Enum.Parse(context.Type, x)} - {x}")));

        model.Example = new OpenApiInteger(0);
    }
}