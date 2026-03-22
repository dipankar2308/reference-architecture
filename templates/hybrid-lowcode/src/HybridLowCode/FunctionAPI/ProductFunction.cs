using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;

namespace FunctionAPI;

public class ProductFunction
{
    [Function("GetProduct")]
    public IActionResult GetProduct(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequest req,
        string id)
    {
        // Delegate to Application layer via service injection
        return new OkObjectResult(new { id, name = "Product Name" });
    }

    [Function("CreateProduct")]
    public async Task<IActionResult> CreateProduct(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequest req)
    {
        var body = await JsonSerializer.DeserializeAsync<CreateProductRequest>(req.Body);
        if (body is null) return new BadRequestObjectResult("Invalid request body.");
        var id = Guid.NewGuid();
        return new OkObjectResult(new { id, body.Name });
    }
}

record CreateProductRequest(string Name, decimal Price);
