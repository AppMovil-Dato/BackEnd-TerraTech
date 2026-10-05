using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.StockManagement.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.StockManagement.Interfaces.REST.Transform;
public static class InventoryResourceFromEntityAssembler
{
    public static InventoryResource ToResourceFromEntity(Inventory entity)
    {
        return new InventoryResource(entity.Id, entity.ProductId, entity.StockQuantity, entity.WarehouseLocation, entity.CreatedAt);
    }
}
