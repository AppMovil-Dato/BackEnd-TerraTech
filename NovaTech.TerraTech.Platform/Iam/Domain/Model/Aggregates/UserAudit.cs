using NovaTech.TerraTech.Platform.Shared.Domain.Model.Entities;

namespace NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;
public partial class User : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
