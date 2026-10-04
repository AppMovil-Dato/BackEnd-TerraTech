namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
/// <summary>Provisioned physical identity; association is unique on Device.SensorCode.</summary>
public class SensorCatalogItem
{
    public string SensorCode { get; set; } = null!;
    public string MacAddress { get; set; } = null!;
    public bool IsDemo { get; set; }
}
