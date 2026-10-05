namespace NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Commands;
public record CreateCommentCommand(int AuthorProfileId, int TargetProfileId, string Content, int Rating);
