namespace NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.ValueObjects;
public record CommunityNickname(string Nickname)
{
    public CommunityNickname() : this(string.Empty)
    {
    }
}
