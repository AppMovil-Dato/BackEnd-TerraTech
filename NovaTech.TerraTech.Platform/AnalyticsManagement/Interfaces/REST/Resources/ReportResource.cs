using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "A Report resource")]
public record ReportResource([SwaggerParameter(Description = "Server-generated ID")] int Id, [SwaggerParameter(Description = "Device ID")] int DeviceId, [SwaggerParameter(Description = "Generation date")] DateTimeOffset GeneratedAt, [SwaggerParameter(Description = "Mean value")] double MeanValue, [SwaggerParameter(Description = "Variance")] double Variance, [SwaggerParameter(Description = "Standard deviation")] double StandardDeviation, [SwaggerParameter(Description = "Technical interpretation")] string TechnicalInterpretation);
