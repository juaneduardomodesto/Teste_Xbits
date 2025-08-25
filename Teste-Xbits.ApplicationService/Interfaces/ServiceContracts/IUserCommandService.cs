using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IUserCommandService
{
    Task<bool> RegisterUserAsync(UserRegisterRequest dtoRegister);
    Task<bool> UpdateUserAsync(UserUpdateRequest dtoUpdate);
    Task<bool> DeleteUserAsync(UserDeleteRequest dtoDelete);
}