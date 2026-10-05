namespace NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.ValueObjects;
public record ContactPhone(string Number)
{
    public ContactPhone() : this(string.Empty)
    {
    }
}
