using NovaTech.TerraTech.Platform.CommercialManagement.Application.Errors;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.Shared.Application.Model;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Application.Services;
public interface IOrderService
{
    Task<Result<Order>> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<Order>> Handle(ValidateOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<Order>> Handle(UpdateOrderStatusCommand command, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> Handle(GetAllOrdersQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> Handle(GetOrdersByProfileQuery query, CancellationToken cancellationToken = default);
    Task<Order?> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken = default);
}
