using Application.Ports;
using Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Persistence;

public class CosmosProductRepository : IProductRepository
{
    private readonly Container _container;

    public CosmosProductRepository(CosmosClient client, string dbName)
        => _container = client.GetContainer(dbName, "products");

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var res = await _container.ReadItemAsync<Product>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: ct);
            return res.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
    {
        var query = _container.GetItemQueryIterator<Product>();
        var results = new List<Product>();
        while (query.HasMoreResults)
        {
            var page = await query.ReadNextAsync(ct);
            results.AddRange(page);
        }
        return results;
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
        => await _container.CreateItemAsync(product, cancellationToken: ct);

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
        => await _container.UpsertItemAsync(product, cancellationToken: ct);

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        => await _container.DeleteItemAsync<Product>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: ct);
}
