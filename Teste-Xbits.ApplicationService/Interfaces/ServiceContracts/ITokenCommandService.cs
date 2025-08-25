using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface ITokenCommandService
{
    public Task<TokenResponse?> Authentication(LoginRequest dtoLogin);
}