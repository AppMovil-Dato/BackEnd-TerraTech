using NovaTech.TerraTech.Platform.Shared.Domain.Model.Entities;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Aggregates;
public partial class Profile : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
