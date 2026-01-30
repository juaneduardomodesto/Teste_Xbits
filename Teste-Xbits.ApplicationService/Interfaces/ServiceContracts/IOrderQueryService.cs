using Teste_Xbits.ApplicationService.DataTransferObjects.Response.OrderResponse;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IOrderQueryService
{
    Task<OrderResponse?> GetOrderByIdAsync(long orderId);
    Task<OrderResponse?> GetOrderByNumberAsync(string orderNumber);
    Task<PageList<OrderResponse>> GetUserOrdersAsync(long userId, PageParams pageParams);
}