# Event-Driven Microservices Template

## Architecture

Command Service -> Service Bus -> Query Service -> Cosmos DB

## Key Components

- Azure Service Bus (Topics/Subscriptions)
- .NET Minimal APIs
- MassTransit
- Cosmos DB Change Feed Processor

## Event Contract Example

```csharp
public record ProductCreatedEvent(Guid ProductId, string Name);
```

## Includes

- Saga orchestration
- Idempotency handling
- Dead letter queue processing
