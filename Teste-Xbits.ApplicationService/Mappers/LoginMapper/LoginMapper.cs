using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

namespace Teste_Xbits.ApplicationService.Mappers.LoginMapper;

public class LoginMapper : ILoginMapper
{
    public LoginResponse DtoToLoginResponse(LoginRequest dtoLogin, TokenResponse tokenResponse) =>
        new()
        {
            Username = dtoLogin.Email,
            Token = tokenResponse.AccessToken,
            ExpireIn = tokenResponse.ExpireIn,
        };
}