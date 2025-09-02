using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IProductCategoryCommandService
{
    Task<bool> RegisterAsync(ProductCategoryRegisterRequest dtoRegister, UserCredential userCredential);
    Task<bool> UpdateRegisterAsync(ProductCategoryUpdateRequest dtoUpdate, UserCredential userCredential);
    Task<bool> DeleteRegisterAsync(ProductCategoryDeleteRequest dtoDelete, UserCredential userCredential);
}