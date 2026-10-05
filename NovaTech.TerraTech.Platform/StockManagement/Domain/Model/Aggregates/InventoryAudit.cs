using NovaTech.TerraTech.Platform.Shared.Domain.Model.Entities;

namespace NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Aggregates;
public partial class Inventory : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
