using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

public sealed class CloudRunServer(string connection, bool migrateOnStartup = false) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");
        builder.UseSetting("Database:MigrateOnStartup", migrateOnStartup.ToString());
        builder.UseSetting("K_SERVICE", "terratech-test");
        builder.UseSetting("ConnectionStrings:DefaultConnection", connection);
        builder.UseSetting("TokenSettings:Secret", TestServer.Secret);
    }
}
