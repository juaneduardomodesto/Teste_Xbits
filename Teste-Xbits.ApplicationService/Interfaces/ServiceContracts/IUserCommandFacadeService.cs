using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IUserCommandFacadeService
{
    Task<bool> RegisterUserAsync(UserRegisterRequest dtoRegister, Guid userId, bool firstUser);
}