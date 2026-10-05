using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Community profile resource")]
public record CommunityProfileResource([SwaggerParameter(Description = "Profile database identifier")] int Id, [SwaggerParameter(Description = "Profile identifier")] int ProfileId, [SwaggerParameter(Description = "Nickname")] string Nickname, [SwaggerParameter(Description = "Reputation score")] int ReputationScore, [SwaggerParameter(Description = "Public biography")] string PublicBio, [SwaggerParameter(Description = "Visibility status")] VisibilityStatus VisibilityStatus);
