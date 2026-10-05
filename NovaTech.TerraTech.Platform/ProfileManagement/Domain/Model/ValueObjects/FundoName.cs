namespace NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.ValueObjects;
public record FundoName(string Name)
{
    public FundoName() : this(string.Empty)
    {
    }
}
