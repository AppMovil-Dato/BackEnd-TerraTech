using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Aggregates;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
public static class ModelBuilderExtensions
{
    public static void ApplyProfileManagementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Profile>().HasKey(p => p.Id);
        builder.Entity<Profile>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Profile>().Property(p => p.UserId).IsRequired();
        builder.Entity<Profile>().OwnsOne(p => p.Name, n =>
        {
            n.WithOwner().HasForeignKey("Id");
            n.Property(x => x.Name).HasColumnName("FundoName");
        });
        builder.Entity<Profile>().OwnsOne(p => p.Phone, p =>
        {
            p.WithOwner().HasForeignKey("Id");
            p.Property(x => x.Number).HasColumnName("ContactPhone");
        });
        builder.Entity<Profile>().OwnsOne(p => p.Thresholds, t =>
        {
            t.WithOwner().HasForeignKey("Id");
            t.Property(x => x.Moisture).HasColumnName("MoistureThreshold");
            t.Property(x => x.Temperature).HasColumnName("TempThreshold");
        });
    }
}
