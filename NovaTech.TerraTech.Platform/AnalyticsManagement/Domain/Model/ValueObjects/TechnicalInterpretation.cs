namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.ValueObjects;
public sealed record TechnicalInterpretation
{
    private const int MaxLength = 500;
    public TechnicalInterpretation(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("TechnicalInterpretation cannot be null, empty, or whitespace.", nameof(value));
        if (value.Length > MaxLength)
            throw new ArgumentException($"TechnicalInterpretation cannot be longer than {MaxLength} characters.", nameof(value));
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
}
