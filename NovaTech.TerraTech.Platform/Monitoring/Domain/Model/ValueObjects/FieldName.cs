namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
public sealed record FieldName
{
    private const int MaxLength = 100;
    public FieldName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("FieldName cannot be null, empty, or whitespace.", nameof(value));
        if (value.Length > MaxLength)
            throw new ArgumentException($"FieldName cannot be longer than {MaxLength} characters.", nameof(value));
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
}
