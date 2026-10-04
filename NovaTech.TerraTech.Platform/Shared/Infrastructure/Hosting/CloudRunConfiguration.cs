using Microsoft.AspNetCore.HttpOverrides;

namespace NovaTech.TerraTech.Platform.Shared.Infrastructure.Hosting;

public static class CloudRunConfiguration
{
    public static bool IsCloudRunService(IConfiguration configuration) =>
        !string.IsNullOrWhiteSpace(configuration["K_SERVICE"]);

    public static void ConfigureCloudRun(this WebApplicationBuilder builder)
    {
        var portValue = builder.Configuration["PORT"];
        if (!string.IsNullOrWhiteSpace(portValue))
        {
            if (!int.TryParse(portValue, out var port) || port is < 1 or > 65535)
                throw new InvalidOperationException("PORT must be an integer between 1 and 65535.");
            builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
        }

        if (!IsCloudRunService(builder.Configuration)) return;

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            // Cloud Run controls the public ingress and terminates TLS. Only accept
            // the scheme: forwarded client IPs and hosts are not trusted here.
            options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            options.ForwardLimit = 1;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });
    }
}
