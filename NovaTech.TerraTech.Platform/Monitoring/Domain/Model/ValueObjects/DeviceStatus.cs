namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
public sealed record DeviceStatus
{
    public string Value { get; }

    private DeviceStatus(string value) => Value = value;
    public static DeviceStatus Online => new("ONLINE");
    public static DeviceStatus Offline => new("OFFLINE");
    public static DeviceStatus LowBattery => new("LOW_BATTERY");

    public static DeviceStatus Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Status cannot be null or empty.", nameof(value));
        var normalized = value.ToUpperInvariant();
        if (normalized is not "ONLINE" and not "OFFLINE" and not "LOW_BATTERY")
            throw new ArgumentException("Status must be ONLINE, OFFLINE, or LOW_BATTERY.", nameof(value));
        return new DeviceStatus(normalized);
    }

    public override string ToString() => Value;
}
