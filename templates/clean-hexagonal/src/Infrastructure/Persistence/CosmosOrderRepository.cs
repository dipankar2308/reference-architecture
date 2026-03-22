using Application.Ports;
using Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Persistence;

public class CosmosOrderRepository : IOrderRepository
{
    private readonly Container _container;

    public CosmosOrderRepository(CosmosClient client, string dbName)
        => _container = client.GetContainer(dbName, "orders");

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var res = await _container.ReadItemAsync<Order>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: ct);
            return res.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<Order>> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        var query = _container.GetItemQueryIterator<Order>(
            new QueryDefinition("SELECT * FROM c WHERE c.ProductId = @id").WithParameter("@id", productId));
        var results = new List<Order>();
        while (query.HasMoreResults)
        {
            var page = await query.ReadNextAsync(ct);
            results.AddRange(page);
        }
        return results;
    }

    public async Task AddAsync(Order order, CancellationToken ct = default)
        => await _container.CreateItemAsync(order, cancellationToken: ct);

    public async Task UpdateAsync(Order order, CancellationToken ct = default)
        => await _container.UpsertItemAsync(order, cancellationToken: ct);
}
