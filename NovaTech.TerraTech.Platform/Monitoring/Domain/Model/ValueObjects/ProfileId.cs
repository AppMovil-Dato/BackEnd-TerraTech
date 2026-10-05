namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
public sealed record ProfileId
{
    public ProfileId(int value)
    {
        if (value <= 0)
            throw new ArgumentException("ProfileId must be a positive integer.", nameof(value));
        Value = value;
    }

    public int Value { get; }

    public override string ToString() => Value.ToString();
}
