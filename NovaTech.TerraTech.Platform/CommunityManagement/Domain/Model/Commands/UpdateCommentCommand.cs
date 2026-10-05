namespace NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Commands;
public record UpdateCommentCommand(int Id, string Content, int Rating);
