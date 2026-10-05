using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.StockManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to create an inventory item")]
public record CreateInventoryResource([Required][SwaggerParameter(Description = "Product identifier")] int ProductId, [Required][SwaggerParameter(Description = "Stock quantity")] int StockQuantity, [SwaggerParameter(Description = "Warehouse location")] string? WarehouseLocation = null);
