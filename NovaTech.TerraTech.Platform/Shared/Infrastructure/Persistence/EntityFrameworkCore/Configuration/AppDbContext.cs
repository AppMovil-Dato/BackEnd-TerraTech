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
public class AppDbContext(DbContextOptions options, IHttpContextAccessor? accessor = null) : DbContext(options)
{
    public int CurrentUserId => accessor?.HttpContext?.User.UserId() ?? 0;

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyIamConfiguration();
        builder.ApplyCommercialConfiguration();
        builder.ApplyNotificationConfiguration();
        builder.ApplyMonitoringConfiguration();
        builder.ApplyProfileManagementConfiguration();
        builder.ConfigureCommunityManagementContext();
        builder.ApplyAnalyticsManagementConfiguration();
        builder.ApplyStockConfiguration();
        builder.ConfigureTb1(this);
        builder.UseSnakeCaseNamingConvention();
    }
}
