using Microsoft.Extensions.Logging;
using NovaTech.TerraTech.Platform.Shared.Application.Model;
using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;
using NovaTech.TerraTech.Platform.StockManagement.Application.Errors;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.StockManagement.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.StockManagement.Application.Services;
public class StockService(IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork, ILogger<StockService> logger) : IStockService
{
    public async Task<Result<Inventory>> Handle(CreateInventoryCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var inventory = new Inventory(command);
            await inventoryRepository.AddAsync(inventory, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Inventory>.Success(inventory);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating inventory for ProductId {ProductId}", command.ProductId);
            return Result<Inventory>.Failure(StockError.InvalidProductId, "The provided product data is invalid");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating inventory for ProductId {ProductId}", command.ProductId);
            return Result<Inventory>.Failure(StockError.UnexpectedError, "An unexpected error occurred while creating the inventory");
        }
    }

    public async Task<Result<Inventory>> Handle(UpdateInventoryCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var inventory = await inventoryRepository.FindByIdAsync(command.Id, cancellationToken);
            if (inventory == null)
                return Result<Inventory>.Failure(StockError.NotFound, "The inventory item was not found");
            if (command.StockQuantity < 0)
                return Result<Inventory>.Failure(StockError.InvalidStockQuantity, "Stock quantity cannot be negative");
            inventory.UpdateStock(command.StockQuantity);
            inventoryRepository.Update(inventory);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Inventory>.Success(inventory);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating inventory {Id}", command.Id);
            return Result<Inventory>.Failure(StockError.UnexpectedError, "An unexpected error occurred while updating the inventory");
        }
    }

    public async Task<IEnumerable<Inventory>> Handle(GetAllInventoryQuery query, CancellationToken cancellationToken = default)
    {
        return await inventoryRepository.ListAsync(cancellationToken);
    }

    public async Task<Inventory?> Handle(GetInventoryByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await inventoryRepository.FindByIdAsync(query.Id, cancellationToken);
    }
}
