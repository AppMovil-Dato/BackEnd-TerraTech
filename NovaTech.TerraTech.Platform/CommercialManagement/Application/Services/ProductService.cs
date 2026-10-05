using Microsoft.Extensions.Logging;
using NovaTech.TerraTech.Platform.CommercialManagement.Application.Errors;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Repositories;
using NovaTech.TerraTech.Platform.Shared.Application.Model;
using NovaTech.TerraTech.Platform.Shared.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Application.Services;
public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, ILogger<ProductService> logger) : IProductService
{
    public async Task<Result<Product>> Handle(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = new Product(command);
            await productRepository.AddAsync(product, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Product>.Success(product);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating product");
            return Result<Product>.Failure(CommercialError.InvalidProductId, "The provided product data is invalid");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating product");
            return Result<Product>.Failure(CommercialError.UnexpectedError, "An unexpected error occurred while creating the product");
        }
    }

    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken = default)
    {
        return await productRepository.ListAsync(cancellationToken);
    }

    public async Task<Product?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await productRepository.FindByIdAsync(query.Id, cancellationToken);
    }
}
