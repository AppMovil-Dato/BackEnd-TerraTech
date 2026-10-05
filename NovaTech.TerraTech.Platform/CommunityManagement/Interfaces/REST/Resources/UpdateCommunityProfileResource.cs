using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to update a community profile")]
public record UpdateCommunityProfileResource([Required][SwaggerParameter(Description = "Nickname")] string Nickname, [Required][SwaggerParameter(Description = "Reputation score")] int ReputationScore, [Required][SwaggerParameter(Description = "Public biography")] string PublicBio, [Required][SwaggerParameter(Description = "Visibility status")] VisibilityStatus VisibilityStatus);
