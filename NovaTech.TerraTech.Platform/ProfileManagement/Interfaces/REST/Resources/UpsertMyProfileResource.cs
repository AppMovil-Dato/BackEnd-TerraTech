using System.ComponentModel.DataAnnotations;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Interfaces.REST.Resources;

public record UpsertMyProfileResource(
    [Required, MinLength(2), MaxLength(150)] string FullName,
    [Required, MaxLength(100)] string FundoName,
    [Required, MaxLength(30)] string ContactPhone,
    [Required, MaxLength(250)] string Location,
    [Range(0.01, 999999999)] double SizeM2,
    [Range(0, 100)] double? MoistureThreshold = null,
    double? TempThreshold = null
);
