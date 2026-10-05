using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.Shared.Application.Model;

namespace NovaTech.TerraTech.Platform.Monitoring.Application.Services;
public interface IFieldCommandService
{
    Task<Result<Field>> Handle(CreateFieldCommand command, CancellationToken cancellationToken = default);
    Task<Result<Field>> Handle(UpdateFieldCommand command, CancellationToken cancellationToken = default);
    Task<Result<Field>> Handle(DeleteFieldCommand command, CancellationToken cancellationToken = default);
}
