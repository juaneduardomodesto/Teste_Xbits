using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface ILoginMapper
{
    LoginResponse DtoToLoginResponse(LoginRequest dtoLogin, TokenResponse tokenResponse);
}