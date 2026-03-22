using Application.Ports;
using Domain.Entities;
using Domain.Events;
using MediatR;

namespace Application.UseCases.Products;

public record CreateProductCommand(string Name, decimal Price) : IRequest<Guid>;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repository;
    private readonly IEventPublisher _publisher;

    public CreateProductHandler(IProductRepository repository, IEventPublisher publisher)
        => (_repository, _publisher) = (repository, publisher);

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = Product.Create(request.Name, request.Price);
        await _repository.AddAsync(product, ct);
        await _publisher.PublishAsync(new ProductCreatedEvent(product.Id, product.Name, product.Price, DateTimeOffset.UtcNow), ct);
        return product.Id;
    }
}
