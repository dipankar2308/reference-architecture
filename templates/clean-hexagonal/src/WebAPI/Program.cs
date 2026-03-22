using Azure.Messaging.ServiceBus;
using Infrastructure.Caching;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Microsoft.Azure.Cosmos;
using StackExchange.Redis;
using WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Azure Cosmos DB
builder.Services.AddSingleton(_ => new CosmosClient(builder.Configuration["CosmosDb:ConnectionString"]));
builder.Services.AddScoped<Application.Ports.IProductRepository>(
    sp => new CosmosProductRepository(sp.GetRequiredService<CosmosClient>(), builder.Configuration["CosmosDb:Database"]!));
builder.Services.AddScoped<Application.Ports.IOrderRepository>(
    sp => new CosmosOrderRepository(sp.GetRequiredService<CosmosClient>(), builder.Configuration["CosmosDb:Database"]!));

// Azure Service Bus
builder.Services.AddSingleton(_ => new ServiceBusClient(builder.Configuration["ServiceBus:ConnectionString"]));
builder.Services.AddScoped<Application.Ports.IEventPublisher>(
    sp => new ServiceBusEventPublisher(sp.GetRequiredService<ServiceBusClient>(), "domain-events"));

// Redis Cache
builder.Services.AddSingleton<IConnectionMultiplexer>(
    _ => ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]!));
builder.Services.AddSingleton<Application.Ports.ICacheService, RedisCacheService>();

// MediatR + FluentValidation
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.UseCases.Products.CreateProductCommand).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
