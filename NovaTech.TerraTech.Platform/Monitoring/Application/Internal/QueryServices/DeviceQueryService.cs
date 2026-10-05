using NovaTech.TerraTech.Platform.Monitoring.Application.Services;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Repositories;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;

namespace NovaTech.TerraTech.Platform.Monitoring.Application.Internal.QueryServices;
public class DeviceQueryService(IDeviceRepository deviceRepository, ILogger<DeviceQueryService> logger) : IDeviceQueryService
{
    public async Task<Device?> GetDeviceByIdAsync(int deviceId, CancellationToken cancellationToken = default) => await deviceRepository.FindByIdAsync(deviceId, cancellationToken);
    public async Task<IEnumerable<Device>> GetAllDevicesAsync(CancellationToken cancellationToken = default) => await deviceRepository.ListAsync(cancellationToken);
    public async Task<IEnumerable<Device>> GetDevicesByFieldIdAsync(int fieldId, CancellationToken cancellationToken = default) => await deviceRepository.FindByFieldIdAsync(new FieldId(fieldId), cancellationToken);
    public async Task<IEnumerable<Device>> GetDevicesByStatusAsync(DeviceStatus status, CancellationToken cancellationToken = default) => await deviceRepository.FindByStatusAsync(status, cancellationToken);
    public async Task<IEnumerable<DeviceResource>> GetAllDeviceResourcesAsync(CancellationToken cancellationToken = default)
    {
        var devices = await deviceRepository.ListAsync(cancellationToken);
        var resources = new List<DeviceResource>();
        foreach (var device in devices)
        {
            try
            {
                var resource = DeviceResourceFromEntityAssembler.ToResourceFromEntity(device);
                resources.Add(resource);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Skipping device with Id {DeviceId} due to mapping error", device?.Id);
            }
        }

        return resources;
    }
}
