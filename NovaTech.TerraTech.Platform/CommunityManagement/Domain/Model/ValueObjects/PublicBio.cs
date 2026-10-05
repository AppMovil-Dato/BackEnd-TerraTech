namespace NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.ValueObjects;
public record PublicBio(string Bio)
{
    public PublicBio() : this(string.Empty)
    {
    }
}
