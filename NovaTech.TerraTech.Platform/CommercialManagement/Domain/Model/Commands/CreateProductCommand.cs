namespace NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Commands;
public record CreateProductCommand(string Name, string Description, decimal Price, string Type, string ImageUrl);
