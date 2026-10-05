using System.Text.Json.Serialization;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST.Resources;
public record UpdateProfileResource([property: JsonPropertyName("fundo_name")] string FundoName, [property: JsonPropertyName("contact_phone")] string ContactPhone, [property: JsonPropertyName("moisture_threshold")] double MoistureThreshold, [property: JsonPropertyName("temp_threshold")] double TempThreshold);
