using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace NovaTech.TerraTech.Platform.Shared.Tb1;
public class SwaggerSecurityFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var anonymous = context.MethodInfo.IsDefined(typeof(AllowAnonymousAttribute), true) || context.MethodInfo.DeclaringType!.IsDefined(typeof(AllowAnonymousAttribute), true);
        if (anonymous) operation.Security = [];
        operation.Responses ??= new OpenApiResponses();
        if (!anonymous) operation.Responses.TryAdd("401", new OpenApiResponse { Description = "JWT missing, invalid, expired, or user no longer exists. Problem Details." });
        operation.Responses.TryAdd("400", new OpenApiResponse { Description = "Invalid input. Problem Details." });
        operation.Responses.TryAdd("404", new OpenApiResponse { Description = "Resource missing or belongs to another account. NO_READINGS distinguishes an empty sensor." });
        if (context.ApiDescription.HttpMethod != "GET") operation.Responses.TryAdd("409", new OpenApiResponse { Description = "Duplicate, occupied sensor, or dependent resources. Problem Details." });
    }
}
