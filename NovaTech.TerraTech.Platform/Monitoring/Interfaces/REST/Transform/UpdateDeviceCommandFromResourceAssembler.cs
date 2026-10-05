using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;
public static class UpdateDeviceCommandFromResourceAssembler
{
    public static UpdateDeviceCommand ToCommandFromResource(int id, UpdateDeviceResource resource)
    {
        return new UpdateDeviceCommand(id, new MacAddress(resource.MacAddress), DeviceStatus.Create(resource.Status), resource.LastSync);
    }
}
