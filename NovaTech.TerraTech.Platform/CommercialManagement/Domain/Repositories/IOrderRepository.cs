using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Domain.Repositories;
public interface IOrderRepository : IBaseRepository<Order>
{
    Task<IEnumerable<Order>> FindByProfileIdAsync(int profileId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> FindByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
}
