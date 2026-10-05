using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Repositories;
public interface IDeviceRepository : IBaseRepository<Device>
{
    Task<IEnumerable<Device>> FindByFieldIdAsync(FieldId fieldId, CancellationToken cancellationToken = default);
    Task<Device?> FindByMacAddressAsync(MacAddress macAddress, CancellationToken cancellationToken = default);
    Task<bool> ExistsByMacAddressAsync(MacAddress macAddress, CancellationToken cancellationToken = default);
    Task<IEnumerable<Device>> FindByStatusAsync(DeviceStatus status, CancellationToken cancellationToken = default);
}
