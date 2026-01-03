using Teste_Xbits.ApplicationService.DataTransferObjects.Request.OrderRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.OrderResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.OrderService;

public class OrderCommandService(
    INotificationHandler notification,
    IValidate<Order> validateOrder,
    IValidate<OrderItem> validateOrderItem,
    ILoggerHandler logger,
    IOrderRepository orderRepository,
    ICartRepository cartRepository,
    IOrderMapper orderMapper)
    : ServiceBase<Order>(notification, validateOrder, logger), IOrderCommandService
{
    private readonly INotificationHandler _notificationHandler = notification;
    private readonly IValidate<OrderItem> _validateOrderItem = validateOrderItem;

    public async Task<OrderResponse?> CheckoutAsync(
        CheckoutRequest request,
        UserCredential userCredential)
    {
        var cart = await cartRepository.GetActiveCartWithItemsAsync(userCredential.Id);
        if (cart == null || !cart.Items.Any())
        {
            _notificationHandler.CreateNotification(
                OrderTracer.Checkout,
                "Carrinho vazio ou não encontrado");
            return null;
        }
        
        var order = orderMapper.CreateOrderFromCart(cart, request, userCredential.Id);
        if (!await EntityValidationAsync(order))
            return null;
        
        await orderRepository.SaveAsync(order);
        
        foreach (var cartItem in cart.Items)
        {
            var orderItem = orderMapper.CreateOrderItemFromCartItem(order.Id, cartItem);
            if (!await EntityValidationAsync(orderItem, _validateOrderItem))
                return null;

            await orderRepository.AddOrderItemAsync(orderItem);
        }
        
        var updatedCart = orderMapper.MarkCartAsCheckedOut(cart);
        var result = await cartRepository.UpdateAsync(updatedCart);
        if(result)
            GenerateLogger(OrderTracer.Checkout, userCredential.Id, order.Id.ToString());
        
        var savedOrder = await orderRepository.GetOrderWithItemsAsync(order.Id);
        return savedOrder != null ? orderMapper.DomainToResponse(savedOrder) : null;
    }

    public async Task<bool> ProcessPaymentAsync(
        ProcessPaymentRequest request,
        UserCredential userCredential)
    {
        #region Validations

        if (request.OrderId <= 0)
        {
            _notificationHandler.CreateNotification(
                OrderTracer.Payment,
                EMessage.InvalidId.GetDescription().FormatTo("OrderId"));
            return false;
        }

        #endregion

        var order = await orderRepository.FindByPredicateAsync(x => x.Id == request.OrderId);
        if (order == null)
        {
            _notificationHandler.CreateNotification(
                OrderTracer.Payment,
                EMessage.NotFound.GetDescription().FormatTo("Pedido"));
            return false;
        }
        
        var updatedOrder = orderMapper.MarkOrderAsPaid(order);
        if (!await EntityValidationAsync(updatedOrder))
            return false;

        var result = await orderRepository.UpdateAsync(updatedOrder);
        if (result)
            GenerateLogger(OrderTracer.Payment, userCredential.Id, order.Id.ToString());

        return result;
    }

    public async Task<bool> CancelOrderAsync(
        long orderId,
        string reason,
        UserCredential userCredential)
    {
        var order = await orderRepository.FindByPredicateAsync(x => x.Id == orderId);
        if (order == null)
        {
            _notificationHandler.CreateNotification(
                OrderTracer.Cancel,
                EMessage.NotFound.GetDescription().FormatTo("Pedido"));
            return false;
        }

        if (order.Status == EOrderStatus.Delivered || order.Status == EOrderStatus.Cancelled)
        {
            _notificationHandler.CreateNotification(
                OrderTracer.Cancel,
                "Pedido não pode ser cancelado neste status");
            return false;
        }
        
        var updatedOrder = orderMapper.CancelOrder(order, reason);
        if (!await EntityValidationAsync(updatedOrder))
            return false;

        var result = await orderRepository.UpdateAsync(updatedOrder);
        if (result)
            GenerateLogger(OrderTracer.Cancel, userCredential.Id, order.Id.ToString());

        return result;
    }
}