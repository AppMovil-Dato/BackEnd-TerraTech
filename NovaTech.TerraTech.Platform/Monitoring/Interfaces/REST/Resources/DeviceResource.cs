using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

/// <summary>
/// Resource representing a Device (flattened).
/// </summary>
/// <param name="Id">Database ID.</param>
/// <param name="FieldId">Field ID.</param>
/// <param name="MacAddress">MAC address.</param>
/// <param name="Status">Device status.</param>
/// <param name="LastSync">Last synchronization timestamp.</param>
/// <remarks>
/// Author: Guillermo Howard Robles - u202222275
/// </remarks>
[SwaggerSchema(Description = "A Device resource")]
public record DeviceResource(
    [SwaggerParameter(Description = "Database ID")] int Id,
    [SwaggerParameter(Description = "Field ID")] int FieldId,
    [SwaggerParameter(Description = "MAC address")] string MacAddress,
    [SwaggerParameter(Description = "Status")] string Status,
    [SwaggerParameter(Description = "Last synchronization")] DateTimeOffset LastSync,
    string? SensorCode = null, string? Name = null
);