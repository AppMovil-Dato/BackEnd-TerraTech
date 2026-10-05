using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.Commands;
public record CreateReportCommand(DeviceId DeviceId, GeneratedAt GeneratedAt, MeanValue MeanValue, Variance Variance, StandardDeviation StandardDeviation, TechnicalInterpretation TechnicalInterpretation);
