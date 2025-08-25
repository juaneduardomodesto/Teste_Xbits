using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.UserResponse;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface IUserMapper
{
    User DtoRegisterToDomain(UserRegisterRequest dtoRegister);
    User DtoUpdateToDomain(UserUpdateRequest dtoUpdate, long userId);
    UserResponse DomainToSimpleResponse(User user);
    public PageList<UserResponse> DomainToPaginationUserResponse(PageList<User> userPageList);
}