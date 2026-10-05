using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Commands;

namespace NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Aggregates;
public partial class Inventory
{
    protected Inventory()
    {
    }

    public Inventory(CreateInventoryCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ProductId = command.ProductId;
        StockQuantity = command.StockQuantity;
        WarehouseLocation = command.WarehouseLocation ?? string.Empty;
    }

    public int? OwnerUserId { get; private set; }

    public void AssignOwner(int userId) => OwnerUserId = userId;
    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public int StockQuantity { get; private set; }
    public string WarehouseLocation { get; private set; }

    public void UpdateStock(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(newQuantity));
        StockQuantity = newQuantity;
    }

    public void DiscountStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Discount quantity must be greater than zero", nameof(quantity));
        if (StockQuantity < quantity)
            throw new InvalidOperationException("Insufficient stock");
        StockQuantity -= quantity;
    }
}
