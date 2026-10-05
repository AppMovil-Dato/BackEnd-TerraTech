using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.StockManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to update an inventory item")]
public record UpdateInventoryResource([Required][SwaggerParameter(Description = "Stock quantity")] int StockQuantity);
