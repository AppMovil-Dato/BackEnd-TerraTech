using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Commands;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Aggregates;
public partial class Product
{
    protected Product()
    {
    }

    public Product(CreateProductCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        Price = command.Price;
        Type = command.Type;
        ImageUrl = command.ImageUrl;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public string Type { get; private set; }
    public string? ImageUrl { get; private set; }
}
