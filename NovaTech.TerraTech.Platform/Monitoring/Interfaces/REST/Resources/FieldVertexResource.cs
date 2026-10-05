using System.ComponentModel.DataAnnotations;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;
public record FieldVertexResource([Range(-90, 90)] double Latitude, [Range(-180, 180)] double Longitude);
