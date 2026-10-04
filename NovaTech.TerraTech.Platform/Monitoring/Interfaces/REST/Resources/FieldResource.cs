using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;

/// <summary>
/// Resource representing a Field (flattened).
/// </summary>
/// <param name="Id">Database ID.</param>
/// <param name="ProfileId">Profile ID.</param>
/// <param name="Name">Field name.</param>
/// <param name="SizeM2">Size in square meters.</param>
/// <param name="SoilType">Soil type.</param>
/// <param name="Latitude">Latitude coordinate.</param>
/// <param name="Longitude">Longitude coordinate.</param>
/// <remarks>
/// Author: Guillermo Howard Robles - u202222275
/// </remarks>
[SwaggerSchema(Description = "A Field resource")]
public record FieldResource(
    [SwaggerParameter(Description = "Database ID")] int Id,
    [SwaggerParameter(Description = "Profile ID")] int ProfileId,
    [SwaggerParameter(Description = "Field name")] string Name,
    [SwaggerParameter(Description = "Size in square meters")] double SizeM2,
    [SwaggerParameter(Description = "Soil type")] string SoilType,
    [SwaggerParameter(Description = "Latitude")] double Latitude,
    [SwaggerParameter(Description = "Longitude")] double Longitude,
    string? CropName = null
);