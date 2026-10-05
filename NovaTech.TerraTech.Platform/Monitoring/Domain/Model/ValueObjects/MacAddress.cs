using System.Text.RegularExpressions;

namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
public sealed record MacAddress
{
    private const int MaxLength = 17;
    private static readonly Regex MacRegex = new Regex(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$", RegexOptions.Compiled);
    public MacAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("MAC address cannot be null or empty.", nameof(value));
        if (value.Length > MaxLength)
            throw new ArgumentException($"MAC address cannot be longer than {MaxLength} characters.", nameof(value));
        if (!MacRegex.IsMatch(value))
            throw new ArgumentException("MAC address must be in format XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX.", nameof(value));
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;
}
