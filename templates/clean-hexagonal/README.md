# Clean Architecture + Hexagonal Template

## Structure

```ascii
src/
├── Domain/           # Pure business logic
├── Application/      # Use cases + ports
├── Infrastructure/   # Adapters (Azure, DB)
└── WebAPI/           # Entry point
```

## Key Files

**`Domain/Entities/Product.cs`**

```csharp
public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    private Product() { }
    public static Product Create(string name) => new() { Id = Guid.NewGuid(), Name = name };
}
```

**`Application/Ports/IProductRepository.cs`**

```csharp
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(Product product);
}
```

**`Infrastructure/AzureProductRepository.cs`**

```csharp
public class AzureProductRepository : IProductRepository
{
    private readonly CosmosClient _cosmos;
    public async Task<Product?> GetByIdAsync(Guid id) { /* Cosmos impl */ }
    public async Task AddAsync(Product product) { /* Cosmos impl */ }
}
```

## Deployment

- ARM/Bicep templates for Cosmos DB + App Service
- Ready for AKS deployment
- GitHub Actions CI/CD pipeline
