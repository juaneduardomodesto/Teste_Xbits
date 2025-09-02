using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductRequest;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IProductCommandService
{
    Task<bool> RegisterProductAsync(ProductRegisterRequest dtoRegister, UserCredential userCredential);
    Task<bool> UpdateProductAsync(ProductUpdateRequest dtoUpdate, UserCredential userCredential);
    Task<bool> DeleteProductAsync(ProductDeleteRequest dtoDelete, UserCredential userCredential);
}