namespace NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.ValueObjects;
public record ProfileThresholds(double Moisture, double Temperature)
{
    public ProfileThresholds() : this(0, 0)
    {
    }

    public ProfileThresholds(double moisture) : this(moisture, 0)
    {
    }

    public string ThresholdSummary => $"Humedad: {Moisture}%, Temperatura: {Temperature}°C";
}
