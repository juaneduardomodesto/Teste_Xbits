using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface ITokenCommandService
{
    public Task<TokenResponse?> AuthenticationAsync(LoginRequest dtoLogin, long userId, ERoles  role);
}