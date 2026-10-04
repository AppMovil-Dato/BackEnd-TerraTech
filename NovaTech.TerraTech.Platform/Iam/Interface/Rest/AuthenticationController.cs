using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Iam.Domain.Model.ValueObjects;
using NovaTech.TerraTech.Platform.Iam.Interface.Rest.Resources;
using NovaTech.TerraTech.Platform.Iam.Application.Internal.OutboundServices;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
namespace NovaTech.TerraTech.Platform.Iam.Interface.Rest;
[ApiController, Route("api/v1/authentication"), AllowAnonymous]
public class AuthenticationController(NovaTech.TerraTech.Platform.Iam.Application.Internal.CommandServices.AccountService accounts) : ControllerBase
{
    [HttpPost("sign-up"), ProducesResponseType<UserResource>(201)]
    public async Task<IActionResult> SignUp(SignUpResource resource, CancellationToken ct)
    {
        var user = await accounts.Register(resource.FullName, resource.EmailAddress, resource.Password, resource.ConfirmPassword, ct);
        return Created($"/api/v1/users/{user.Id}", new UserResource(user.Id, user.EmailAddress.Value, user.FullName));
    }
    [HttpPost("sign-in"), ProducesResponseType<AuthenticatedUserResource>(200)]
    public async Task<IActionResult> SignIn(SignInResource resource, CancellationToken ct)
    {
        var (user, token) = await accounts.Login(resource.EmailAddress, resource.Password, ct);
        return Ok(new AuthenticatedUserResource(user.Id, user.EmailAddress.Value, token, user.FullName, new JsonWebToken(token).ValidTo));
    }
}
