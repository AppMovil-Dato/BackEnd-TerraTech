namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;
public record ReadingResource(int Id, int DeviceId, DateTime RecordedAt, double MoisturePercent, double SoilTemperatureC, double NitrogenPpm, double PhosphorusPpm, double PotassiumPpm, string Source);
