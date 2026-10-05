using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NovaTech.TerraTech.Platform.Shared.Tb1;
public class RecordRequiredSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema concrete)
            return;
        foreach (var parameter in context.Type.GetConstructors().SelectMany(c => c.GetParameters()))
        {
            if (parameter.GetCustomAttribute<RequiredAttribute>()is null || parameter.Name is null)
                continue;
            var name = JsonNamingPolicy.CamelCase.ConvertName(parameter.Name);
            if (concrete.Properties?.ContainsKey(name) != true)
                continue;
            concrete.Required ??= new HashSet<string>();
            concrete.Required.Add(name);
        }
    }
}
