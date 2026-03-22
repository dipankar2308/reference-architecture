namespace Contracts;

public record ProductCreatedEvent(
    Guid ProductId,
    string Name,
    decimal Price,
    DateTimeOffset OccurredAt);
