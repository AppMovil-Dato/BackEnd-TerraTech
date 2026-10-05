using NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Iam.Interface.Rest.Resources;

namespace NovaTech.TerraTech.Platform.Iam.Interface.Rest.Transform;
public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User aggregate cannot be null when creating authenticated user resource.");
        }

        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentNullException("Token cannot be null or empty when creating authenticated user resource.", nameof(token));
        }

        return new AuthenticatedUserResource(user.Id, user.EmailAddress.Value, token);
    }
}
