using NovaTech.TerraTech.Platform.Iam.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.Iam.Interface.Rest.Resources;

namespace NovaTech.TerraTech.Platform.Iam.Interface.Rest.Transform;
public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        if (resource == null)
        {
            throw new ArgumentNullException(nameof(resource), "SignInResource cannot be null when converting to command.");
        }

        return new SignInCommand(resource.EmailAddress, resource.Password);
    }
}
