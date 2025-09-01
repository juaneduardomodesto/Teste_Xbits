using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductResponse;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.UserResponse;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IProductQueryService
{
    Task<ProductResponse?> FindByIdAsync(long id);
    Task<PageList<ProductResponse>> FindAllWithPaginationAsync(
        string? namePrefix,
        string? descriptionPrefix,
        decimal? pricePrefix,
        string? productCodePrefix,
        bool? hasValidadeDatePrefix,
        string? expirationDate,
        long? productCategoryIdPrefix,
        PageParams pageParams);
}