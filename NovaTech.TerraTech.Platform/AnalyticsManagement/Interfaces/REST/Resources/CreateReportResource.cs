using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to create a Report")]
public record CreateReportResource([Required][SwaggerParameter(Description = "Device ID (positive integer)")] int DeviceId, [Required][SwaggerParameter(Description = "Date and time when the report was generated (ISO format)")] DateTimeOffset GeneratedAt, [Required][Range(0, 100)][SwaggerParameter(Description = "Mean value (0-100)")] double MeanValue, [Required][Range(0, double.MaxValue)][SwaggerParameter(Description = "Variance (non-negative)")] double Variance, [Required][Range(0, double.MaxValue)][SwaggerParameter(Description = "Standard deviation (non-negative)")] double StandardDeviation, [Required][MaxLength(500)][SwaggerParameter(Description = "Technical interpretation (max 500 chars)")] string TechnicalInterpretation);
