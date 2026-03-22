namespace Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Product() { }

    public static Product Create(string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (price <= 0) throw new Exceptions.DomainException("Price must be positive.");
        return new Product { Id = Guid.NewGuid(), Name = name, Price = price };
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0) throw new Exceptions.DomainException("Price must be positive.");
        Price = newPrice;
    }

    public void Deactivate() => IsActive = false;
}
