using Teste_Xbits.ApplicationService.DataTransferObjects.Response.CartResponse;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface ICartQueryService
{
    Task<CartResponse?> GetActiveCartAsync(long userId);
    Task<CartResponse?> GetCartByIdAsync(long cartId);
}