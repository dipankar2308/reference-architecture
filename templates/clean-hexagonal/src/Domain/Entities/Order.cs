namespace Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private Order() { }

    public static Order Place(Guid productId, int quantity)
    {
        if (quantity <= 0) throw new Exceptions.DomainException("Quantity must be positive.");
        return new Order
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Quantity = quantity,
            Status = OrderStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Confirm() => Status = OrderStatus.Confirmed;
    public void Cancel() => Status = OrderStatus.Cancelled;
}

public enum OrderStatus { Pending, Confirmed, Cancelled }
