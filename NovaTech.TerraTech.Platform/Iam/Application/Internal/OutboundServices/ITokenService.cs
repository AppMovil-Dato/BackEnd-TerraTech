using NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;

namespace NovaTech.TerraTech.Platform.Iam.Application.Internal.OutboundServices;
public interface ITokenService
{
    string GenerateToken(User user);
    Task<int?> ValidateToken(string token);
}
