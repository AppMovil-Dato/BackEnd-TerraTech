using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;
public static class DeviceResourceFromEntityAssembler
{
    public static DeviceResource ToResourceFromEntity(Device entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        var fieldId = entity.FieldId?.Value ?? 0;
        var macAddress = entity.MacAddress?.Value ?? string.Empty;
        var status = entity.Status?.Value ?? string.Empty;
        var lastSync = entity.LastSync?.Value ?? DateTimeOffset.MinValue;
        return new DeviceResource(entity.Id, fieldId, macAddress, status, lastSync, entity.SensorCode, entity.Name);
    }
}
