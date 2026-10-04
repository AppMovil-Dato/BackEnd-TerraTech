using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;

namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;

public partial class Device
{
    protected Device()
    {
        FieldId = null!;
        MacAddress = null!;
        Status = null!;
        LastSync = null!;
    }

    public Device(CreateDeviceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        
        FieldId = command.FieldId;
        MacAddress = command.MacAddress;
        Status = command.Status;
        LastSync = new LastSync(command.LastSync);
    }
    
    public string? SensorCode { get; private set; }
    public string? Name { get; private set; }
    public void Identify(string code, string name) { SensorCode = code; Name = name.Trim(); }
    public int Id { get; private set; }
    public FieldId FieldId { get; private set; }
    public MacAddress MacAddress { get; private set; }
    public DeviceStatus Status { get; private set; }
    public LastSync LastSync { get; private set; }
    
    public void Update(UpdateDeviceCommand command)
    {
        MacAddress = command.MacAddress;
        Status = command.Status;
        LastSync = new LastSync(command.LastSync);
    }
}