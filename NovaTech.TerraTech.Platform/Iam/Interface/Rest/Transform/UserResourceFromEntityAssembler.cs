using NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Iam.Interface.Rest.Resources;

namespace NovaTech.TerraTech.Platform.Iam.Interface.Rest.Transform;
public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User aggregate cannot be null when converting to resource.");
        }

        return new UserResource(user.Id, user.EmailAddress.Value);
    }
}
