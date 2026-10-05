using NovaTech.TerraTech.Platform.Monitoring.Application.Services;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Repositories;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;
using NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;

namespace NovaTech.TerraTech.Platform.Monitoring.Application.Internal.QueryServices;
public class FieldQueryService(IFieldRepository fieldRepository, ILogger<FieldQueryService> logger) : IFieldQueryService
{
    public async Task<Field?> GetFieldByIdAsync(int fieldId, CancellationToken cancellationToken = default) => await fieldRepository.FindByIdAsync(fieldId, cancellationToken);
    public async Task<IEnumerable<Field>> GetAllFieldsAsync(CancellationToken cancellationToken = default) => await fieldRepository.ListAsync(cancellationToken);
    public async Task<IEnumerable<Field>> GetFieldsBySoilTypeAsync(SoilType soilType, CancellationToken cancellationToken = default) => await fieldRepository.FindBySoilTypeAsync(soilType, cancellationToken);
    public async Task<IEnumerable<FieldResource>> GetAllFieldResourcesAsync(CancellationToken cancellationToken = default)
    {
        var fields = await fieldRepository.ListAsync(cancellationToken);
        var resources = new List<FieldResource>();
        foreach (var field in fields)
        {
            try
            {
                var resource = FieldResourceFromEntityAssembler.ToResourceFromEntity(field);
                resources.Add(resource);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Skipping field with Id {FieldId} due to mapping error", field?.Id);
            }
        }

        return resources;
    }
}
