using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Aggregates;
public partial class Profile
{
    public Profile()
    {
        UserId = 0;
        Name = new FundoName();
        Phone = new ContactPhone();
        Thresholds = new ProfileThresholds();
    }

    public Profile(int userId, string fundoName, string contactPhone, double moistureThreshold, double tempThreshold)
    {
        UserId = userId;
        Name = new FundoName(fundoName);
        Phone = new ContactPhone(contactPhone);
        Thresholds = new ProfileThresholds(moistureThreshold, tempThreshold);
    }

    public Profile(CreateProfileCommand command)
    {
        UserId = command.UserId;
        Name = new FundoName(command.FundoName);
        Phone = new ContactPhone(command.ContactPhone);
        Thresholds = new ProfileThresholds(command.MoistureThreshold, command.TempThreshold);
    }

    public void Update(UpdateProfileCommand command)
    {
        Name = new FundoName(command.FundoName);
        Phone = new ContactPhone(command.ContactPhone);
        Thresholds = new ProfileThresholds(command.MoistureThreshold, command.TempThreshold);
    }

    public string? Location { get; private set; }
    public double? SizeM2 { get; private set; }

    public void SetTerrain(string location, double sizeM2)
    {
        Location = location.Trim();
        SizeM2 = sizeM2;
    }

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public FundoName Name { get; private set; }
    public ContactPhone Phone { get; private set; }
    public ProfileThresholds Thresholds { get; private set; }
    public string FundoNameString => Name.Name;
    public string ContactPhoneString => Phone.Number;
    public double MoistureThresholdValue => Thresholds.Moisture;
    public double TempThresholdValue => Thresholds.Temperature;
}
