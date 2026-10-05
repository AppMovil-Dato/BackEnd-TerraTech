using System.Text.RegularExpressions;

namespace NovaTech.TerraTech.Platform.Iam.Domain.Model.ValueObjects;
public sealed partial record Email
{
    [GeneratedRegex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex EmailValidationRegex();
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be null o whitespace.", nameof(value));
        }

        if (!EmailValidationRegex().IsMatch(value))
        {
            throw new ArgumentException("Email format is invalid.", nameof(value));
        }

        Value = value.ToLowerInvariant().Trim();
    }

    public string Value { get; init; }

    public override string ToString() => Value;
}
