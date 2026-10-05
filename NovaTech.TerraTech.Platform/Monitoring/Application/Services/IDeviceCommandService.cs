using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.Shared.Application.Model;

namespace NovaTech.TerraTech.Platform.Monitoring.Application.Services;
public interface IDeviceCommandService
{
    Task<Result<Device>> Handle(CreateDeviceCommand command, CancellationToken cancellationToken = default);
    Task<Result<Device>> Handle(UpdateDeviceCommand command, CancellationToken cancellationToken = default);
    Task<Result<Device>> Handle(DeleteDeviceCommand command, CancellationToken cancellationToken = default);
}
