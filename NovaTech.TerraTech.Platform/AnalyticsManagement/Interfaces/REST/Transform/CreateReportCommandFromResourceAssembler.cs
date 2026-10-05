using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.AnalyticsManagement.Interfaces.REST.Resources;

namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Interfaces.REST.Transform;
public static class CreateReportCommandFromResourceAssembler
{
    public static CreateReportCommand ToCommandFromResource(CreateReportResource resource)
    {
        return new CreateReportCommand(new DeviceId(resource.DeviceId), new GeneratedAt(resource.GeneratedAt), new MeanValue(resource.MeanValue), new Variance(resource.Variance), new StandardDeviation(resource.StandardDeviation), new TechnicalInterpretation(resource.TechnicalInterpretation));
    }
}
