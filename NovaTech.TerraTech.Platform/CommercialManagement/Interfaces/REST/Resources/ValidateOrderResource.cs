using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to validate an order")]
public record ValidateOrderResource([Required][SwaggerParameter(Description = "Order identifier")] int OrderId);
