using Teste_Xbits.ApplicationService.DataTransferObjects.Request.OrderRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.OrderResponse;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface IOrderMapper
{
    OrderResponse DomainToResponse(Order order);
    PageList<OrderResponse> DomainToPaginationResponse(PageList<Order> orderPageList);
    Order CreateOrderFromCart(Cart cart, CheckoutRequest request, long userId);
    OrderItem CreateOrderItemFromCartItem(long orderId, CartItem cartItem);
    Cart MarkCartAsCheckedOut(Cart cart);
    Order MarkOrderAsPaid(Order order);
    Order CancelOrder(Order order, string reason);
}