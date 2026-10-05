namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.ValueObjects;
public sealed record Variance
{
    public Variance(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Variance must be a valid number.", nameof(value));
        if (value < 0)
            throw new ArgumentException("Variance cannot be negative.", nameof(value));
        Value = value;
    }

    public double Value { get; }

    public override string ToString() => Value.ToString();
}
