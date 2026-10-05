using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.Commands
{
    public record UpdateReportCommand(int Id, double MeanValue, double Variance, double StandardDeviation, string TechnicalInterpretation);
}
