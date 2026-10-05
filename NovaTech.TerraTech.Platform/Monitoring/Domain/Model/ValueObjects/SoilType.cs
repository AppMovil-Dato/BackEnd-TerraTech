namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
public sealed record SoilType
{
    private const int MaxLength = 50;
    public SoilType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SoilType cannot be null, empty, or whitespace.", nameof(value));
        if (value.Length > MaxLength)
            throw new ArgumentException($"SoilType cannot be longer than {MaxLength} characters.", nameof(value));
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
}
