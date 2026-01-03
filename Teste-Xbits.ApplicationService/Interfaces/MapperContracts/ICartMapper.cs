using Teste_Xbits.ApplicationService.DataTransferObjects.Request.CartRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.CartResponse;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface ICartMapper
{
    CartResponse DomainToResponse(Cart cart);
    CartItemResponse CartItemToResponse(CartItem cartItem);
    CartItem CreateCartItemFromRequest(long cartId, Product product, int quantity);
    CartItem UpdateCartItemQuantity(CartItem existingItem, int additionalQuantity);
    CartItem UpdateCartItemFromRequest(CartItem cartItem, UpdateCartItemRequest request);
}
