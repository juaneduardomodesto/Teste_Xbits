using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IUserCommandService
{
    Task<bool> RegisterUserAsync(UserRegisterRequest dtoRegister, Guid userId, bool firstUser);
    Task<bool> UpdateUserAsync(UserUpdateRequest dtoUpdate, UserCredential  userCredential);
    Task<bool> DeleteUserAsync(UserDeleteRequest dtoDelete, UserCredential  userCredential);
}