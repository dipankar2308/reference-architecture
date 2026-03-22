using Application.Ports;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Products;

public record GetProductQuery(Guid Id) : IRequest<Product?>;

public class GetProductHandler : IRequestHandler<GetProductQuery, Product?>
{
    private readonly IProductRepository _repository;
    private readonly ICacheService _cache;

    public GetProductHandler(IProductRepository repository, ICacheService cache)
        => (_repository, _cache) = (repository, cache);

    public async Task<Product?> Handle(GetProductQuery request, CancellationToken ct)
    {
        var cached = await _cache.GetAsync<Product>($"product:{request.Id}", ct);
        if (cached is not null) return cached;
        var product = await _repository.GetByIdAsync(request.Id, ct);
        if (product is not null)
            await _cache.SetAsync($"product:{product.Id}", product, TimeSpan.FromMinutes(10), ct);
        return product;
    }
}
