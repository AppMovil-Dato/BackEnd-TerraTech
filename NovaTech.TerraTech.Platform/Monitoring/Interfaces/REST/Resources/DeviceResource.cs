using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;
[SwaggerSchema(Description = "A Device resource")]
public record DeviceResource([SwaggerParameter(Description = "Database ID")] int Id, [SwaggerParameter(Description = "Field ID")] int FieldId, [SwaggerParameter(Description = "MAC address")] string MacAddress, [SwaggerParameter(Description = "Status")] string Status, [SwaggerParameter(Description = "Last synchronization")] DateTimeOffset LastSync, string? SensorCode = null, string? Name = null);
