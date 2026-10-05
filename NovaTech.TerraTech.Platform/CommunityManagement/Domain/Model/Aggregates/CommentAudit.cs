using NovaTech.TerraTech.Platform.Shared.Domain.Model.Entities;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Aggregates;
public partial class Comment : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
