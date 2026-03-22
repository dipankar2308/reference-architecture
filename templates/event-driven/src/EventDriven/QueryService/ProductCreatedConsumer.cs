using Contracts;
using MassTransit;

public class ProductCreatedConsumer : IConsumer<ProductCreatedEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var evt = context.Message;
        // Persist to read model (Cosmos DB)
        // var readModel = new ProductReadModel(evt.ProductId, evt.Name, evt.Price);
        // await _readRepository.UpsertAsync(readModel);
        await Task.CompletedTask;
    }
}
