using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Commands;
public record UpdateOrderStatusCommand(int OrderId, OrderStatus NewStatus);
