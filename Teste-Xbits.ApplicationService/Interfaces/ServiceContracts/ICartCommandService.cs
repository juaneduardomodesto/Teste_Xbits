using Teste_Xbits.ApplicationService.DataTransferObjects.Request.CartRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.CartResponse;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface ICartCommandService
{
    Task<CartItemResponse?> AddToCartAsync(AddToCartRequest request, UserCredential userCredential);
    Task<bool> UpdateCartItemAsync(UpdateCartItemRequest request, UserCredential userCredential);
    Task<bool> RemoveFromCartAsync(RemoveFromCartRequest request, UserCredential userCredential);
    Task<bool> ClearCartAsync(UserCredential userCredential);
}