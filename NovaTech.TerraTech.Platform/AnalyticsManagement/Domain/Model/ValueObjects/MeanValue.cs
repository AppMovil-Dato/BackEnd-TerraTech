namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.ValueObjects;
public sealed record MeanValue
{
    public MeanValue(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("MeanValue must be a valid number.", nameof(value));
        if (value < 0 || value > 100)
            throw new ArgumentException("MeanValue must be between 0 and 100.", nameof(value));
        Value = value;
    }

    public double Value { get; }

    public override string ToString() => Value.ToString();
}
