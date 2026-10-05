using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Domain.Repositories;
public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}
