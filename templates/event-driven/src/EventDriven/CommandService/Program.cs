using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.UsingAzureServiceBus((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["ServiceBus:ConnectionString"]);
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

app.MapPost("/products", async (CreateProductRequest req, IPublishEndpoint bus) =>
{
    var id = Guid.NewGuid();
    await bus.Publish(new Contracts.ProductCreatedEvent(id, req.Name, req.Price, DateTimeOffset.UtcNow));
    return Results.Created($"/products/{id}", new { id });
});

app.MapPost("/orders", async (PlaceOrderRequest req, IPublishEndpoint bus) =>
{
    var id = Guid.NewGuid();
    await bus.Publish(new Contracts.OrderPlacedEvent(id, req.ProductId, req.Quantity, DateTimeOffset.UtcNow));
    return Results.Created($"/orders/{id}", new { id });
});

app.Run();

record CreateProductRequest(string Name, decimal Price);
record PlaceOrderRequest(Guid ProductId, int Quantity);
