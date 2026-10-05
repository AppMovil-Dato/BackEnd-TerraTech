using NovaTech.TerraTech.Platform.Shared.Application.Model;
using NovaTech.TerraTech.Platform.StockManagement.Application.Errors;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Queries;

namespace NovaTech.TerraTech.Platform.StockManagement.Application.Services;
public interface IStockService
{
    Task<Result<Inventory>> Handle(CreateInventoryCommand command, CancellationToken cancellationToken = default);
    Task<Result<Inventory>> Handle(UpdateInventoryCommand command, CancellationToken cancellationToken = default);
    Task<IEnumerable<Inventory>> Handle(GetAllInventoryQuery query, CancellationToken cancellationToken = default);
    Task<Inventory?> Handle(GetInventoryByIdQuery query, CancellationToken cancellationToken = default);
}
