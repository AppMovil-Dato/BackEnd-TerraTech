namespace NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Commands;
public record CreateInventoryCommand(int ProductId, int StockQuantity, string? WarehouseLocation = null);
