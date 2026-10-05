using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;

namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Repositories;
public interface IFieldRepository : IBaseRepository<Field>
{
    Task<IEnumerable<Field>> FindBySoilTypeAsync(SoilType soilType, CancellationToken cancellationToken = default);
    Task<Field?> FindBySoilTypeAndLocationLatLongAsync(SoilType SoilType, LocationLatLong LocationLatLong, CancellationToken cancellationToken = default);
}
