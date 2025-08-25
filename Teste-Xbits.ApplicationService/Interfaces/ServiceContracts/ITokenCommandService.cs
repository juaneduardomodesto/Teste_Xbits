using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface ITokenCommandService
{
    public Task<TokenResponse?> Authentication(LoginRequest dtoLogin);
}