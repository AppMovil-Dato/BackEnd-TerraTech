using System.ComponentModel.DataAnnotations;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;
public record RegisterSensorResource([Required, RegularExpression("^TT-[0-9A-Z]{6}$")] string SensorCode, [Range(1, int.MaxValue)] int FieldId, [Required, MaxLength(100)] string Name);
