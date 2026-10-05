using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;
[SwaggerSchema(Description = "A Field resource")]
public record FieldResource([SwaggerParameter(Description = "Database ID")] int Id, [SwaggerParameter(Description = "Profile ID")] int ProfileId, [SwaggerParameter(Description = "Field name")] string Name, [SwaggerParameter(Description = "Size in square meters")] double SizeM2, [SwaggerParameter(Description = "Soil type")] string SoilType, [SwaggerParameter(Description = "Latitude")] double Latitude, [SwaggerParameter(Description = "Longitude")] double Longitude, string? CropName = null, IReadOnlyList<FieldVertexResource>? Boundary = null);
