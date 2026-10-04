using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaTech.TerraTech.Platform.CommercialManagement.Application.Services;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.CommercialManagement.Interfaces.REST.Resources;
using NovaTech.TerraTech.Platform.CommercialManagement.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Tags("Commercials")]
public class ProductsController(
    IProductService productService,
    ILogger<ProductsController> logger) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new product", Description = "Creates a product for the catalog")]
    [SwaggerResponse(201, "Product created", typeof(ProductResource))]
    [SwaggerResponse(400, "Invalid request", typeof(string))]
    [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateProductCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await productService.Handle(command, cancellationToken);
            
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetProductById), new { id = result.Value.Id }, 
                    ProductResourceFromEntityAssembler.ToResourceFromEntity(result.Value));
            }
            
            return BadRequest("Invalid product request");
        }
        catch (ApiFailure) { throw; }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating product");
            return Problem(title: "Unexpected server error", 
                detail: "An unexpected error occurred while processing your request", statusCode: 500);
        }
    }

    [AllowAnonymous]
    [HttpGet]
    [SwaggerOperation(Summary = "Gets all products")]
    [SwaggerResponse(200, "Products retrieved", typeof(IEnumerable<ProductResource>))]
    public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken)
    {
        var query = new GetAllProductsQuery();
        var products = await productService.Handle(query, cancellationToken);
        var resources = products.Select(ProductResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets a product by id")]
    [SwaggerResponse(200, "Product found", typeof(ProductResource))]
    [SwaggerResponse(404, "Product not found")]
    public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);
        var product = await productService.Handle(query, cancellationToken);
        
        if (product == null)
            return NotFound();
        
        var resource = ProductResourceFromEntityAssembler.ToResourceFromEntity(product);
        return Ok(resource);
    }
}