namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
public sealed record SizeM2
{
    public SizeM2(double value)
    {
        if (value <= 0)
            throw new ArgumentException("SizeM2 must be greater than zero.", nameof(value));
        if (value > 9999999)
            throw new ArgumentException("SizeM2 cannot exceed 9,999,999 m².", nameof(value));
        Value = value;
    }

    public double Value { get; }

    public override string ToString() => $"{Value} m²";
}
