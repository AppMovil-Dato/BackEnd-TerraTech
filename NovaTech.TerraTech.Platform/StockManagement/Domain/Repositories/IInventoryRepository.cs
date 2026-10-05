using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Aggregates;

namespace NovaTech.TerraTech.Platform.StockManagement.Domain.Repositories;
public interface IInventoryRepository : IBaseRepository<Inventory>
{
    Task<Inventory?> FindByProductIdAsync(int productId, CancellationToken cancellationToken = default);
}
