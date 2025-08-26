using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IProductCategoryCommandService
{
    Task<bool> RegisterAsync(ProductCategoryRegisterRequest dtoRegister);
    Task<bool> UpdateRegisterAsync(ProductCategoryUpdateRequest dtoUpdate);
    Task<bool> DeleteRegisterAsync(ProductCategoryDeleteRequest dtoDelete);
}