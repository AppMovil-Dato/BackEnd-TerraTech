using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.StockManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Inventory resource")]
public record InventoryResource([SwaggerParameter(Description = "Inventory identifier")] int Id, [SwaggerParameter(Description = "Product identifier")] int ProductId, [SwaggerParameter(Description = "Stock quantity")] int StockQuantity, [SwaggerParameter(Description = "Warehouse location")] string WarehouseLocation, [SwaggerParameter(Description = "Creation timestamp")] DateTimeOffset? CreatedAt);
