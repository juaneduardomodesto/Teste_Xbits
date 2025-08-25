using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.LoginResponse;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface ILoginQueryService
{
    Task<LoginResponse?> Login(LoginRequest dtoLogin);
}