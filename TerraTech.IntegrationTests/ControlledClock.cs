public sealed class ControlledClock : TimeProvider
{
    public DateTimeOffset? Frozen { get; set; }
    public override DateTimeOffset GetUtcNow() => Frozen ?? DateTimeOffset.UtcNow;
}

