using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to create a comment")]
public record CreateCommentResource([Required][SwaggerParameter(Description = "Author profile identifier")] int AuthorProfileId, [Required][SwaggerParameter(Description = "Target profile identifier")] int TargetProfileId, [Required][SwaggerParameter(Description = "Content of the comment")] string Content, [Required][SwaggerParameter(Description = "Rating score (0-5)")] int Rating);
