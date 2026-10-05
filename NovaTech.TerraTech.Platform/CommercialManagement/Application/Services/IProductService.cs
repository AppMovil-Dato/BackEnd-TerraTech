using NovaTech.TerraTech.Platform.CommercialManagement.Application.Errors;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.CommercialManagement.Domain.Model.Queries;
using NovaTech.TerraTech.Platform.Shared.Application.Model;

namespace NovaTech.TerraTech.Platform.CommercialManagement.Application.Services;
public interface IProductService
{
    Task<Result<Product>> Handle(CreateProductCommand command, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken = default);
    Task<Product?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken = default);
}
