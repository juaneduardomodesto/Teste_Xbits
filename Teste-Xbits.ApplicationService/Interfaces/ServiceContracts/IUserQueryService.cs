using Teste_Xbits.ApplicationService.DataTransferObjects.Response.UserResponse;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IUserQueryService
{
    //Task<UserResponse?> GetCurrentUserAsync();
    Task<UserResponse?> FindByIdAsync(long id);
    Task<PageList<UserResponse>> FindAllWithPaginationAsync(
        string? namePrefix,
        string? emailPrefix,
        string? cpfPrefix,
        PageParams pageParams);
}