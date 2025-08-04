using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Base.Helpers;

/// <summary>
/// Extension methods and helpers for JWT token handling and user identity extraction.
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    /// Extracts the user's <see cref="Guid"/> identifier from the given <see cref="ClaimsPrincipal"/>.
    /// </summary>
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userIdStr = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userId = Guid.Parse(userIdStr);
        return userId;
    }

    private static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();

    /// <summary>
    /// Generates a JWT token with the given claims and signing information.
    /// </summary>
    public static string GenerateJwt(
        IEnumerable<Claim> claims,
        string key,
        string issuer,
        string audience,
        DateTime expires)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials
        );

        return JwtSecurityTokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Validates the given JWT token using the specified parameters.
    /// </summary>
    public static bool ValidateJwt(string jwt, string key, string issuer, string audience)
    {
        var validationParams = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuerSigningKey = true,

            ValidIssuer = issuer,
            ValidateIssuer = true,

            ValidAudience = audience,
            ValidateAudience = true,

            ValidateLifetime = false
        };

        try
        {
            new JwtSecurityTokenHandler().ValidateToken(jwt, validationParams, out _);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}