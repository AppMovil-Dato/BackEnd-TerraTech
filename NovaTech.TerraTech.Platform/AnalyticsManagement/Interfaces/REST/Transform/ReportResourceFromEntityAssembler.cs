using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.AnalyticsManagement.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Interfaces.REST.Transform;
public static class ReportResourceFromEntityAssembler
{
    public static ReportResource ToResourceFromEntity(Report entity) => new(entity.Id, entity.DeviceId.Value, entity.GeneratedAt.Value, entity.MeanValue.Value, entity.Variance.Value, entity.StandardDeviation.Value, entity.TechnicalInterpretation.Value);
}
