using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductRequest;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IProductCommandService
{
    Task<bool> RegisterProductAsync(ProductRegisterRequest dtoRegister);
    Task<bool> UpdateProductAsync(ProductUpdateRequest dtoUpdate);
    Task<bool> DeleteProductAsync(ProductDeleteRequest dtoDelete);
}