using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface ITokenMapper
{
    TokenResponse MapToTokenResponse(string accessToken, string userIdentifier, int expireIn);
}