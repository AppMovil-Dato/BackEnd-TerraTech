using Microsoft.AspNetCore.Mvc;
using NovaTech.TerraTech.Platform.Shared.Application.Model;
using NovaTech.TerraTech.Platform.StockManagement.Application.Errors;
using NovaTech.TerraTech.Platform.StockManagement.Application.Services;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.StockManagement.Interfaces.REST.Resources;
using NovaTech.TerraTech.Platform.StockManagement.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.StockManagement.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Tags("Stocks")]
public class InventoriesController(
    IStockService stockService,
    ILogger<InventoriesController> logger) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new inventories item", Description = "Creates an inventories item for a product")]
    [SwaggerResponse(201, "Inventory created", typeof(InventoryResource))]
    [SwaggerResponse(400, "Invalid request", typeof(string))]
    [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
    public async Task<IActionResult> CreateInventory([FromBody] CreateInventoryResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = CreateInventoryCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await stockService.Handle(command, cancellationToken);
            
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetInventoryById), new { id = result.Value.Id }, 
                    InventoryResourceFromEntityAssembler.ToResourceFromEntity(result.Value));
            }
            
            return (StockError)result.Error switch
            {
                StockError.InvalidProductId or StockError.InvalidStockQuantity 
                    => BadRequest("Invalid inventory request"),
                _ => Problem(title: "Unexpected server error", 
                    detail: "An unexpected error occurred while processing your request", statusCode: 500)
            };
        }
        catch (ApiFailure) { throw; }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating inventory");
            return Problem(title: "Unexpected server error", 
                detail: "An unexpected error occurred while processing your request", statusCode: 500);
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Updates an inventories item", Description = "Updates the stock quantity of an inventories item")]
    [SwaggerResponse(200, "Inventory updated", typeof(InventoryResource))]
    [SwaggerResponse(404, "Inventory not found", typeof(string))]
    [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateInventory(int id, [FromBody] UpdateInventoryResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateInventoryCommand(id, resource.StockQuantity);
            var result = await stockService.Handle(command, cancellationToken);
            
            if (result.IsSuccess)
            {
                return Ok(InventoryResourceFromEntityAssembler.ToResourceFromEntity(result.Value));
            }
            
            return (StockError)result.Error == StockError.NotFound ? NotFound() : 
                Problem(title: "Unexpected server error", statusCode: 500);
        }
        catch (ApiFailure) { throw; }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating inventory {Id}", id);
            return Problem(title: "Unexpected server error", statusCode: 500);
        }
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all inventories items", Description = "Retrieves all inventories items")]
    [SwaggerResponse(200, "Inventory retrieved", typeof(IEnumerable<InventoryResource>))]
    public async Task<IActionResult> GetAllInventory(CancellationToken cancellationToken)
    {
        var query = new GetAllInventoryQuery();
        var inventory = await stockService.Handle(query, cancellationToken);
        var resources = inventory.Select(InventoryResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets an inventories item by id", Description = "Retrieves an inventories item by its unique identifier")]
    [SwaggerResponse(200, "Inventory found", typeof(InventoryResource))]
    [SwaggerResponse(404, "Inventory not found")]
    public async Task<IActionResult> GetInventoryById(int id, CancellationToken cancellationToken)
    {
        var query = new GetInventoryByIdQuery(id);
        var inventory = await stockService.Handle(query, cancellationToken);
        
        if (inventory == null)
            return NotFound();
        
        var resource = InventoryResourceFromEntityAssembler.ToResourceFromEntity(inventory);
        return Ok(resource);
    }
}