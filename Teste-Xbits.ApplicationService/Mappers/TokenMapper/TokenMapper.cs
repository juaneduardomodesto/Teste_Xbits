using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Mappers.TokenMapper;

public class TokenMapper : ITokenMapper
{
    public TokenResponse MapToTokenResponse(string accessToken, string username, int expireIn)
    {
        return new TokenResponse
        {
            AccessToken = accessToken,
            Username = username,
            ExpireIn = expireIn,
        };
    }
}