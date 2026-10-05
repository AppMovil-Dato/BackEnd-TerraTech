namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.ValueObjects;
public sealed record StandardDeviation
{
    public StandardDeviation(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("StandardDeviation must be a valid number.", nameof(value));
        if (value < 0)
            throw new ArgumentException("StandardDeviation cannot be negative.", nameof(value));
        Value = value;
    }

    public double Value { get; }

    public override string ToString() => Value.ToString();
}
