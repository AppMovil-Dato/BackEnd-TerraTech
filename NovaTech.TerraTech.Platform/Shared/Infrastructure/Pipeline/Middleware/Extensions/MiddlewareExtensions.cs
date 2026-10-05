using NovaTech.TerraTech.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

namespace NovaTech.TerraTech.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
