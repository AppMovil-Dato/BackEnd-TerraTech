using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;

/// <summary>
/// Assembler for converting <see cref="Device"/> entities to <see cref="DeviceResource"/>.
/// </summary>
/// <remarks>
/// Author: Guillermo Howard Robles - u202222275
/// </remarks>
public static class DeviceResourceFromEntityAssembler
{
    /// <summary>
    /// Converts a Device entity to a DeviceResource, handling null values gracefully.
    /// </summary>
    /// <param name="entity">The Device entity.</param>
    /// <returns>A DeviceResource with safe default values for null properties.</returns>
    public static DeviceResource ToResourceFromEntity(Device entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        // Safe extraction with null checks and default values
        var fieldId = entity.FieldId?.Value ?? 0;
        var macAddress = entity.MacAddress?.Value ?? string.Empty;
        var status = entity.Status?.Value ?? string.Empty;
        var lastSync = entity.LastSync?.Value ?? DateTimeOffset.MinValue;

        return new DeviceResource(
            entity.Id,
            fieldId,
            macAddress,
            status,
            lastSync, entity.SensorCode, entity.Name
        );
    }
}