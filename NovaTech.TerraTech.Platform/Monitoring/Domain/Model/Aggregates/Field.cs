using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Commands;

namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;

public partial class Field
{
    protected Field()
    {
        ProfileId = null!;
        Name = null!;
        SizeM2 = null!;
        SoilType = null!;
        LocationLatLong = null!;
    }

    public Field(CreateFieldCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        
        ProfileId = command.ProfileId;
        Name = command.Name;
        SizeM2 = command.SizeM2;
        SoilType = command.SoilType;
        LocationLatLong = command.LocationLatLong;
        CropName = command.CropName;
    }
    
    public string? CropName { get; private set; }
    public void SetCrop(string? name) => CropName = name?.Trim();
    public int Id { get; private set; }
    public ProfileId ProfileId { get; private set; }
    public FieldName Name { get; private set; }
    public SizeM2 SizeM2 { get; private set; }
    public SoilType SoilType { get; private set; }
    public LocationLatLong LocationLatLong { get; private set; }
    
    public void Update(UpdateFieldCommand command)
    {
        Name = command.Name;
        SizeM2 = command.SizeM2;
        SoilType = command.SoilType;
        LocationLatLong = command.LocationLatLong;
        CropName = command.CropName;
    }
}