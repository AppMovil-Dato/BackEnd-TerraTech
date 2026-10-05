using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Commands;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Aggregates;
public partial class Comment
{
    protected Comment()
    {
    }

    public Comment(CreateCommentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        AuthorProfileId = command.AuthorProfileId;
        TargetProfileId = command.TargetProfileId;
        Content = command.Content;
        Rating = command.Rating;
    }

    public int Id { get; private set; }
    public int AuthorProfileId { get; private set; }
    public int TargetProfileId { get; private set; }
    public string Content { get; private set; }
    public int Rating { get; private set; }

    public void UpdateContent(string content, int rating)
    {
        Content = content;
        Rating = rating;
    }
}
