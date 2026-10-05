namespace NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.ValueObjects;
public record Reputation(int Score)
{
    public Reputation() : this(0)
    {
    }
}
