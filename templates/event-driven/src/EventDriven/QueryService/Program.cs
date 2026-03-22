using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductCreatedConsumer>();
    x.UsingAzureServiceBus((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["ServiceBus:ConnectionString"]);
        cfg.SubscriptionEndpoint<Contracts.ProductCreatedEvent>("query-service", e =>
        {
            e.ConfigureConsumer<ProductCreatedConsumer>(ctx);
        });
    });
});

var app = builder.Build();
app.MapGet("/products/{id:guid}", (Guid id) => Results.Ok(new { id, name = "Read from query store" }));
app.Run();
