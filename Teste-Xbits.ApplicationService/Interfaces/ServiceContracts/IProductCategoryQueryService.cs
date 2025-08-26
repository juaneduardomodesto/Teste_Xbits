using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductCategoryResponse;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IProductCategoryQueryService
{
    Task<ProductCategoryResponse?> FindByIdAsync(long id);
    Task<PageList<ProductCategoryResponse>> FindAllWithPaginationAsync(
        string? namePrefix,
        string? descriptionPrefix,
        string? codePrefix,
        PageParams pageParams);
}