using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Hosting;
using MySql.Data.MySqlClient;
using Xunit;

public partial class JourneyTests
{
    [Fact]
    public async Task ProductionMigrationCommandExitsSuccessfullyAndIsRepeatable()
    {
        var first = await Command("Production", "--migrate-only");
        Assert.True(first.exit == 0, first.output);
        var second = await Command("Production", "--migrate-only");
        Assert.True(second.exit == 0, second.output);
    }

    [Fact]
    public async Task CloudRunUsesForwardedHttpsForCreatedResourceLocations()
    {
        var account = await Account();
        await using var production = new CloudRunServer(server.Connection);
        var client = production.CreateClient();
        client.DefaultRequestHeaders.Authorization = account.client.DefaultRequestHeaders.Authorization;
        client.DefaultRequestHeaders.Add("X-Forwarded-Proto", "https");
        var response = await client.PostAsJsonAsync("/api/v1/fields", new { profileId = account.profileId, name = "Cloud Run South", sizeM2 = 5000, soilType = "SANDY", latitude = -12.1, longitude = -77.1, cropName = "Potato" });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("https", response.Headers.Location!.Scheme);
    }

    [Fact]
    public async Task CloudRunProductionServesPublicHealthSwaggerAndPreservesJwtProtection()
    {
        await using var production = new CloudRunServer(server.Connection);
        var client = production.CreateClient(new() { AllowAutoRedirect = false });
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/health/live")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/health/ready")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/swagger/v1/swagger.json")).StatusCode);
        await AssertProblem(await client.GetAsync("/api/v1/users/me"), HttpStatusCode.Unauthorized);
        await AssertProblem(await client.GetAsync("/does-not-exist"), HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CloudRunDoesNotMigrateOnStartupAndReadinessRejectsEmptySchema()
    {
        var config = new MySqlConnectionStringBuilder(server.Connection);
        var database = "terratech_tb1_" + Guid.NewGuid().ToString("N");
        await using var mysql = new MySqlConnection(server.Connection);
        await mysql.OpenAsync();
        await new MySqlCommand($"CREATE DATABASE `{database}`", mysql).ExecuteNonQueryAsync();
        config.Database = database;
        await using var production = new CloudRunServer(config.ConnectionString);
        var client = production.CreateClient();
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/health/live")).StatusCode);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, (await client.GetAsync("/health/ready")).StatusCode);
        var count = await new MySqlCommand($"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema='{database}'", mysql).ExecuteScalarAsync();
        Assert.Equal(0, Convert.ToInt32(count));
    }

    [Theory]
    [InlineData("9090", "http://0.0.0.0:9090")]
    [InlineData("8080", "http://0.0.0.0:8080")]
    public void CloudRunPortOverridesDefaultListener(string port, string expected)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["PORT"] = port });
        builder.ConfigureCloudRun();
        Assert.Equal(expected, builder.Configuration["urls"]);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("65536")]
    [InlineData("invalid")]
    public void CloudRunRejectsInvalidPorts(string port)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["PORT"] = port });
        Assert.Throws<InvalidOperationException>(() => builder.ConfigureCloudRun());
    }

    [Fact]
    public void CloudRunHonorsExplicitStartupMigrationSetting()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Production" });
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["K_SERVICE"] = "terratech-test", ["Database:MigrateOnStartup"] = "true"
        });
        Assert.True(DatabaseStartup.ShouldMigrate(builder.Configuration, builder.Environment, []));
        Assert.True(DatabaseStartup.ShouldMigrate(builder.Configuration, builder.Environment, ["--migrate-only"]));
        builder.Configuration["K_SERVICE"] = "";
        builder.Configuration["Database:MigrateOnStartup"] = "false";
        Assert.False(DatabaseStartup.ShouldMigrate(builder.Configuration, builder.Environment, []));
    }
}
