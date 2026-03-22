using Application.Ports;
using Domain.Entities;
using Domain.Events;
using MediatR;

namespace Application.UseCases.Orders;

public record PlaceOrderCommand(Guid ProductId, int Quantity) : IRequest<Guid>;

public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, Guid>
{
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;
    private readonly IEventPublisher _publisher;

    public PlaceOrderHandler(IOrderRepository orders, IProductRepository products, IEventPublisher publisher)
        => (_orders, _products, _publisher) = (orders, products, publisher);

    public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(request.ProductId, ct)
            ?? throw new Domain.Exceptions.DomainException("Product not found.");
        var order = Order.Place(product.Id, request.Quantity);
        await _orders.AddAsync(order, ct);
        await _publisher.PublishAsync(new OrderPlacedEvent(order.Id, order.ProductId, order.Quantity, DateTimeOffset.UtcNow), ct);
        return order.Id;
    }
}
