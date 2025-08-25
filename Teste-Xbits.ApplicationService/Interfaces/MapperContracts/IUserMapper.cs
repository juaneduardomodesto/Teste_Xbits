using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface IUserMapper
{
    User DtoRegisterToDomain(UserRegisterRequest dtoRegister);
}