using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.AnalyticsManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using NovaTech.TerraTech.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using NovaTech.TerraTech.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using NovaTech.TerraTech.Platform.NotificationManagement.Infrastructure.Persistence.EFC.Configuration.Extensions;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using NovaTech.TerraTech.Platform.StockManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using NovaTech.TerraTech.Platform.CommercialManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using NovaTech.TerraTech.Platform.ProfileManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using NovaTech.TerraTech.Platform.CommunityManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options, IHttpContextAccessor? accessor = null) : DbContext(options)
{
    public int CurrentUserId => accessor?.HttpContext?.User.UserId() ?? 0;
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Apply audit timestamp interceptor for all IAuditableEntity implementations
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Identity and Access Management Context
        builder.ApplyIamConfiguration();
        
        // Commercial Management Context
        builder.ApplyCommercialConfiguration();
        
        // Notification Management Context
        builder.ApplyNotificationConfiguration();
        
        // Monitoring Context
        builder.ApplyMonitoringConfiguration();
        
        // Profiles Context
        builder.ApplyProfileManagementConfiguration();
        
        // Community Management Context
        builder.ConfigureCommunityManagementContext();
        
        //Analytics Context
        builder.ApplyAnalyticsManagementConfiguration();
        
        // Stock Management Context
        builder.ApplyStockConfiguration();
        
        builder.ConfigureTb1(this);
        // General Naming Convention for the database objects
        builder.UseSnakeCaseNamingConvention();
    }
}