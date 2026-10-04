namespace NovaTech.TerraTech.Platform.Shared.Infrastructure.Hosting;

public static class DatabaseStartup
{
    public static bool ShouldMigrate(IConfiguration configuration, IHostEnvironment environment, string[] args)
    {
        if (args.Contains("--migrate-only") || args.Any(arg => arg.StartsWith("--demo-"))) return true;
        if (CloudRunConfiguration.IsCloudRunService(configuration)) return false;
        return configuration.GetValue("Database:MigrateOnStartup", environment.IsDevelopment());
    }
}
