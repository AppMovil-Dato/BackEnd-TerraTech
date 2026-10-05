using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.CommercialManagement.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Interfaces.REST.Transform;
public static class OrderResourceFromEntityAssembler
{
    public static OrderResource ToResourceFromEntity(Order order)
    {
        return new OrderResource(order.Id, order.ProfileId, order.ProductId, order.ProductName, order.Quantity, order.TotalAmount, order.Status, order.PaymentMethod, order.CreatedAt ?? DateTimeOffset.Now, order.IsSubscription);
    }
}
