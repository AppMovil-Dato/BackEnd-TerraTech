namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
public sealed record LastSync
{
    public LastSync(DateTimeOffset value)
    {
        Value = value;
    }

    public DateTimeOffset Value { get; }

    public override string ToString() => Value.ToString("O");
}
