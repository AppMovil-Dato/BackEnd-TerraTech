using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Aggregates;
public partial class Order
{
    protected Order()
    {
    }

    public Order(CreateOrderCommand command, Product product)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(product);
        ProfileId = command.ProfileId;
        ProductId = command.ProductId;
        ProductName = product.Name;
        Quantity = command.Quantity;
        TotalAmount = product.Price * command.Quantity;
        Status = OrderStatus.Pending;
        PaymentMethod = command.PaymentMethod;
        IsSubscription = command.IsSubscription;
    }

    public int Id { get; private set; }
    public int ProfileId { get; private set; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public bool IsSubscription { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public void Validate()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Order cannot be validated. Current status: {Status}");
        Status = OrderStatus.Validated;
    }

    public void UpdateStatus(OrderStatus newStatus)
    {
        if (newStatus == OrderStatus.Cancelled && Status == OrderStatus.Completed)
            throw new ArgumentException("Cannot cancel a completed order");
        Status = newStatus;
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Validated)
            throw new InvalidOperationException($"Order cannot be marked as paid. Current status: {Status}");
        Status = OrderStatus.Paid;
    }

    public void Complete()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException($"Order cannot be completed. Current status: {Status}");
        Status = OrderStatus.Completed;
    }
}
