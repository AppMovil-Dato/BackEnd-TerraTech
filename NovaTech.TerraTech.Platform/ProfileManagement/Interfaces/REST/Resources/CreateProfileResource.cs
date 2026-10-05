using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to create a profile")]
public record CreateProfileResource([Required][SwaggerParameter(Description = "User identifier")] int UserId, [Required][SwaggerParameter(Description = "Name of the fundo/farm")] string FundoName, [Required][SwaggerParameter(Description = "Contact phone number")] string ContactPhone, [Required][SwaggerParameter(Description = "Moisture threshold")] double MoistureThreshold, [Required][SwaggerParameter(Description = "Temperature threshold")] double TempThreshold);
