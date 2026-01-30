using Teste_Xbits.ApplicationService.DataTransferObjects.Response.CartResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.CartService;

public class CartQueryService(
    INotificationHandler notification,
    IValidate<Cart> validate,
    ILoggerHandler logger,
    ICartRepository cartRepository,
    ICartMapper cartMapper)
    : ServiceBase<Cart>(notification, validate, logger), ICartQueryService
{
    public async Task<CartResponse?> GetActiveCartAsync(long userId)
    {
        var cart = await cartRepository.GetActiveCartWithItemsAsync(userId);
        return cart != null ? cartMapper.DomainToResponse(cart) : null;
    }

    public async Task<CartResponse?> GetCartByIdAsync(long cartId)
    {
        var cart = await cartRepository.GetCartWithItemsAsync(cartId);
        return cart != null ? cartMapper.DomainToResponse(cart) : null;
    }
}
