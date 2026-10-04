using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NovaTech.TerraTech.Platform.Iam.Application.Internal.OutboundServices;
using NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;

namespace NovaTech.TerraTech.Platform.Iam.Infrastructure.Tokens.Jwt.Services;

/// <summary>
///     Implements Token service.
/// </summary>
/// <remarks>
///     This class is responsible for generate, validated tokens and rules as expired.
/// </remarks>
public class TokenService(IOptions<TokenSettings> tokenSettings) : ITokenService
{
    private readonly TokenSettings _tokenSettings = tokenSettings.Value;

    /// <summary>
    ///     The method to generate token.
    /// </summary>
    /// <param name="user">
    ///     The user for whom the token is generated.
    /// </param>
    /// <returns>
    ///     The token.
    /// </returns>
    public string GenerateToken(User user)
    {
        var secret = _tokenSettings.Secret;
        var key = Encoding.UTF8.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.EmailAddress.Value)
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return token;
    }

    /// <summary>
    ///     The method to validate token.
    /// </summary>
    /// <param name="token">
    ///     The token.
    /// </param>
    /// <returns>
    ///     User validated by token.
    /// </returns>
    public async Task<int?> ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }
        var tokenHandler = new JsonWebTokenHandler();
        var key = Encoding.UTF8.GetBytes(_tokenSettings.Secret);

        try
        {
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });
            if (!tokenValidationResult.IsValid) return null;
            var jwtToken = (JsonWebToken)tokenValidationResult.SecurityToken;
            var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == "sub" || claim.Type == "sid" || claim.Type == ClaimTypes.Sid).Value);
            return userId;
        }
        catch (Exception)
        {
            return null;
        }
    }
}