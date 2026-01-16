using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.LoginResponse;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface ILoginMapper
{
    LoginResponse DtoToLoginResponse(LoginRequest dtoLogin, TokenResponse tokenResponse, User user);
}