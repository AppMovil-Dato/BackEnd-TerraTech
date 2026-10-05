using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Iam.Interface.Rest.Resources;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace NovaTech.TerraTech.Platform.Iam.Interface.Rest;
[ApiController, Route("api/v1/users")]
public class UsersController(NovaTech.TerraTech.Platform.Iam.Application.Internal.CommandServices.AccountService accounts) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<UserResource>> Me(CancellationToken ct) => await GetUser(User.UserId(), null, ct);
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResource>> ById(int id, CancellationToken ct) => await GetUser(id, null, ct);
    [HttpGet("email/{emailAddress}")]
    public async Task<ActionResult<UserResource>> ByEmail(string emailAddress, CancellationToken ct) => await GetUser(User.UserId(), emailAddress, ct);
    private async Task<ActionResult<UserResource>> GetUser(int id, string? email, CancellationToken ct)
    {
        var user = await accounts.GetOwn(User.UserId(), id, email, ct);
        return new UserResource(user.Id, user.EmailAddress.Value, user.FullName);
    }
}
