namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
public class SensorReading
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public DateTime RecordedAt { get; set; }
    public double MoisturePercent { get; set; }
    public double SoilTemperatureC { get; set; }
    public double NitrogenPpm { get; set; }
    public double PhosphorusPpm { get; set; }
    public double PotassiumPpm { get; set; }
    public string Source { get; set; } = "SIMULATED";
}
