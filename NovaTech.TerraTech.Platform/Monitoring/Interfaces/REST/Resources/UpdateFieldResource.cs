using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

[SwaggerSchema(Description = "Request payload to update a Field")]
public record UpdateFieldResource(
    [Required]
    [MaxLength(100)]
    [SwaggerParameter(Description = "Field name (max 100 characters)")] string Name,
    
    [Required]
    [Range(0.01, 9999999)]
    [SwaggerParameter(Description = "Size in square meters")] double SizeM2,
    
    [Required]
    [MaxLength(50)]
    [SwaggerParameter(Description = "Soil type (max 50 characters)")] string SoilType,
    
    [Required]
    [Range(-90, 90)]
    [SwaggerParameter(Description = "Latitude coordinate (-90 to 90)")] double Latitude,
    
    [Required]
    [Range(-180, 180)]
    [SwaggerParameter(Description = "Longitude coordinate (-180 to 180)")] double Longitude,
    [MaxLength(100)] string? CropName = null,
    IReadOnlyList<FieldVertexResource>? Boundary = null
);