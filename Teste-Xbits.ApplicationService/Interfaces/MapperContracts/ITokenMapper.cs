using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface ITokenMapper
{
    TokenResponse MapToTokenResponse(string accessToken, string username, int expireIn);
}