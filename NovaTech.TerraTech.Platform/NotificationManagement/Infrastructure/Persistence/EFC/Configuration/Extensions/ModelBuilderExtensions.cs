using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Aggregates;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Infrastructure.Persistence.EFC.Configuration.Extensions;
public static class ModelBuilderExtensions
{
    public static void ApplyNotificationConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Notification>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Id).IsRequired().ValueGeneratedOnAdd();
            entity.Property(n => n.ProfileId).IsRequired().HasColumnType("int");
            entity.Property(n => n.Title).IsRequired().HasMaxLength(255);
            entity.Property(n => n.Message).IsRequired().HasMaxLength(1000);
            entity.Property(n => n.IsRead).IsRequired();
            entity.Property(n => n.IsAlert).IsRequired();
            entity.Property(n => n.CreatedAt);
            entity.Property(n => n.UpdatedAt);
        });
    }
}
