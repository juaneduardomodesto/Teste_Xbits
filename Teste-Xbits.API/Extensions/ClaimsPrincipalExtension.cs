using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.API.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string GetEmail(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.Name)?.Value!;

    public static Guid GetUserId(this ClaimsPrincipal user) =>
        Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

    public static Guid GetUserIdFromToken(this string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var jwtToken = handler.ReadJwtToken(token);

        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

        return Guid.Parse(userIdClaim!.Value);
    }
    
    public static UserCredential GetUserCredential(this ClaimsPrincipal user)
    {
        var roles = user.FindAll(ClaimTypes.Role);
        
        var id = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        return new UserCredential
        {
            Id = id,
            Roles = roles.Select(r => r.Value).ToList()
        };
    }
}