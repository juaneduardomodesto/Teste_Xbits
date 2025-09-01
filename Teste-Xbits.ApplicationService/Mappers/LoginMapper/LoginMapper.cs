using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.LoginResponse;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

namespace Teste_Xbits.ApplicationService.Mappers.LoginMapper;

public class LoginMapper : ILoginMapper
{
    public LoginResponse DtoToLoginResponse(LoginRequest dtoLogin, TokenResponse tokenResponse) =>
        new()
        {
            UserIdentifier = dtoLogin.Email,
            Token = tokenResponse.AccessToken,
            ExpireIn = tokenResponse.ExpireIn,
        };
}