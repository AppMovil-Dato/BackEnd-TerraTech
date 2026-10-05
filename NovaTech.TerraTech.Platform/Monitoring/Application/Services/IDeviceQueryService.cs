using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.Monitoring.Application.Services;
public interface IDeviceQueryService
{
    Task<Device?> GetDeviceByIdAsync(int deviceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Device>> GetAllDevicesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Device>> GetDevicesByFieldIdAsync(int fieldId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Device>> GetDevicesByStatusAsync(DeviceStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeviceResource>> GetAllDeviceResourcesAsync(CancellationToken cancellationToken = default);
}
