namespace Contracts;

public record OrderPlacedEvent(
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    DateTimeOffset OccurredAt);
