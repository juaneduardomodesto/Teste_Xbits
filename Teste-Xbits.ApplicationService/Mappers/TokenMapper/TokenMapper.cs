using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

namespace Teste_Xbits.ApplicationService.Mappers.TokenMapper;

public class TokenMapper : ITokenMapper
{
    public TokenResponse MapToTokenResponse(string accessToken, string userIdentifier, int expireIn)
    {
        return new TokenResponse
        {
            AccessToken = accessToken,
            UserIdentifier = userIdentifier,
            ExpireIn = expireIn,
        };
    }
}