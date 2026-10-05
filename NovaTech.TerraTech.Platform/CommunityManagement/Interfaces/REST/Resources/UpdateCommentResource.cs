using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to update a comment")]
public record UpdateCommentResource([Required][SwaggerParameter(Description = "Content of the comment")] string Content, [Required][SwaggerParameter(Description = "Rating score (0-5)")] int Rating);
