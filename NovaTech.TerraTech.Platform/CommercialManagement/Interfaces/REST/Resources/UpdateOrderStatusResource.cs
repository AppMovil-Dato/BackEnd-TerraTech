using System.ComponentModel.DataAnnotations;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to update order status")]
public record UpdateOrderStatusResource([Required][SwaggerParameter(Description = "New order status")] OrderStatus Status);
