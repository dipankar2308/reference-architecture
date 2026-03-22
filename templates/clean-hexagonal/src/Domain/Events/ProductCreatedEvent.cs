namespace Domain.Events;

public record ProductCreatedEvent(
    Guid ProductId,
    string Name,
    decimal Price,
    DateTimeOffset OccurredAt);
