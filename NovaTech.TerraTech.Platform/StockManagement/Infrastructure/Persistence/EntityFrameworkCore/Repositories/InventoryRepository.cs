using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.StockManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
public class InventoryRepository(AppDbContext context) : BaseRepository<Inventory>(context), IInventoryRepository
{
    public async Task<Inventory?> FindByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Inventory>().FirstOrDefaultAsync(i => i.ProductId == productId, cancellationToken);
    }
}
