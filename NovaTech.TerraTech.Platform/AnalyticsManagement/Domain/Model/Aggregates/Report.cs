using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.Aggregates;
public partial class Report
{
    protected Report()
    {
        DeviceId = null!;
        GeneratedAt = null!;
        MeanValue = null!;
        Variance = null!;
        StandardDeviation = null!;
        TechnicalInterpretation = null!;
    }

    public Report(CreateReportCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        DeviceId = command.DeviceId;
        GeneratedAt = command.GeneratedAt;
        MeanValue = command.MeanValue;
        Variance = command.Variance;
        StandardDeviation = command.StandardDeviation;
        TechnicalInterpretation = command.TechnicalInterpretation;
    }

    public int Id { get; private set; }
    public DeviceId DeviceId { get; private set; }
    public GeneratedAt GeneratedAt { get; private set; }
    public MeanValue MeanValue { get; private set; }
    public Variance Variance { get; private set; }
    public StandardDeviation StandardDeviation { get; private set; }
    public TechnicalInterpretation TechnicalInterpretation { get; private set; }

    public void UpdateStatistics(double mean, double variance, double stdDev, string interpretation)
    {
        MeanValue = new MeanValue(mean);
        Variance = new Variance(variance);
        StandardDeviation = new StandardDeviation(stdDev);
        TechnicalInterpretation = new TechnicalInterpretation(interpretation);
    }
}
