using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;
public record UpdateDeviceCommand(int Id, MacAddress MacAddress, DeviceStatus Status, DateTimeOffset LastSync);
