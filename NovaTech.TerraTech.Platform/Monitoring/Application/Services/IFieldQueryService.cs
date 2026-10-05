using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.Monitoring.Application.Services;
public interface IFieldQueryService
{
    Task<Field?> GetFieldByIdAsync(int fieldId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Field>> GetAllFieldsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Field>> GetFieldsBySoilTypeAsync(SoilType soilType, CancellationToken cancellationToken = default);
    Task<IEnumerable<FieldResource>> GetAllFieldResourcesAsync(CancellationToken cancellationToken = default);
}
