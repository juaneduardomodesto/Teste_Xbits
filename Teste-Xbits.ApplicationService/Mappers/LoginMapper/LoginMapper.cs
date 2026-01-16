using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.LoginResponse;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Mappers.LoginMapper;

public class LoginMapper : ILoginMapper
{
    public LoginResponse DtoToLoginResponse(
        LoginRequest dtoLogin, TokenResponse tokenResponse, User user) =>
        new()
        {
            UserIdentifier = dtoLogin.Email,
            Name = user.Name,
            Role = user.Role.ToString(),
            Token = tokenResponse.AccessToken,
            ExpireIn = tokenResponse.ExpireIn,
        };
}