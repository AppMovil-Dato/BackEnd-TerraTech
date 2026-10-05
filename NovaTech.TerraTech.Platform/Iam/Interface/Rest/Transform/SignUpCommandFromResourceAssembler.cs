using NovaTech.TerraTech.Platform.Iam.Domain.Model.Commands;
using NovaTech.TerraTech.Platform.Iam.Interface.Rest.Resources;

namespace NovaTech.TerraTech.Platform.Iam.Interface.Rest.Transform;
public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        if (resource == null)
        {
            throw new ArgumentNullException(nameof(resource), "SignUpResource cannot be null when converting to command.");
        }

        return new SignUpCommand(resource.EmailAddress, resource.Password);
    }
}
