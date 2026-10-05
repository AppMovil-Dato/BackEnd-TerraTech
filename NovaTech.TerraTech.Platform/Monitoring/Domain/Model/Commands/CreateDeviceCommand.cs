using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;
public record CreateDeviceCommand(FieldId FieldId, MacAddress MacAddress, DeviceStatus Status, DateTimeOffset LastSync);
