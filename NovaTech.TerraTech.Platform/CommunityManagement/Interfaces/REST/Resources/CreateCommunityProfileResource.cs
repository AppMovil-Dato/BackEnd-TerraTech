using System.ComponentModel.DataAnnotations;
using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to create a community profile")]
public record CreateCommunityProfileResource([Required][SwaggerParameter(Description = "Profile identifier")] int ProfileId, [Required][SwaggerParameter(Description = "Nickname")] string Nickname, [Required][SwaggerParameter(Description = "Reputation score")] int ReputationScore, [Required][SwaggerParameter(Description = "Public biography")] string PublicBio, [Required][SwaggerParameter(Description = "Visibility status")] VisibilityStatus VisibilityStatus);
