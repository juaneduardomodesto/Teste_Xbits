using Teste_Xbits.ApplicationService.DataTransferObjects.Request.OrderRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.OrderResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Mappers.OrderMapper;

public class OrderMapper : IOrderMapper
{
    public OrderResponse DomainToResponse(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            UserId = order.UserId,
            Status = order.Status,
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            Subtotal = order.Subtotal,
            Discount = order.Discount,
            ShippingCost = order.ShippingCost,
            Total = order.Total,
            Items = order.Items.Select(OrderItemToResponse).ToList(),
            CreatedAt = order.CreatedAt,
            PaidAt = order.PaidAt
        };
    }

    public PageList<OrderResponse> DomainToPaginationResponse(PageList<Order> orderPageList)
    {
        var responses = orderPageList.Items.Select(DomainToResponse).ToList();

        return new PageList<OrderResponse>(
            responses,
            orderPageList.TotalCount,
            orderPageList.CurrentPage,
            orderPageList.PageSize);
    }

    public Order CreateOrderFromCart(Cart cart, CheckoutRequest request, long userId)
    {
        var subtotal = cart.Subtotal;
        var total = subtotal - request.Discount + request.ShippingCost;
        var orderNumber = GenerateOrderNumber();

        return new Order
        {
            OrderNumber = orderNumber,
            UserId = userId,
            CartId = cart.Id,
            Status = EOrderStatus.Pending,
            PaymentMethod = request.PaymentMethod,
            PaymentStatus = EPaymentStatus.Pending,
            Subtotal = subtotal,
            Discount = request.Discount,
            ShippingCost = request.ShippingCost,
            Total = total,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public OrderItem CreateOrderItemFromCartItem(long orderId, CartItem cartItem)
    {
        return new OrderItem
        {
            OrderId = orderId,
            ProductId = cartItem.ProductId,
            ProductName = cartItem.Product?.Name ?? "Produto",
            Quantity = cartItem.Quantity,
            UnitPrice = cartItem.UnitPrice,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public Cart MarkCartAsCheckedOut(Cart cart)
    {
        return cart with
        {
            Status = ECartStatus.CheckedOut,
            CheckedOutAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public Order MarkOrderAsPaid(Order order)
    {
        return order with
        {
            Status = EOrderStatus.Confirmed,
            PaymentStatus = EPaymentStatus.Approved,
            PaidAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public Order CancelOrder(Order order, string reason)
    {
        return order with
        {
            Status = EOrderStatus.Cancelled,
            CancelledAt = DateTime.UtcNow,
            CancellationReason = reason,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private OrderItemResponse OrderItemToResponse(OrderItem orderItem)
    {
        return new OrderItemResponse
        {
            Id = orderItem.Id,
            ProductId = orderItem.ProductId,
            ProductName = orderItem.ProductName,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            TotalPrice = orderItem.TotalPrice
        };
    }

    private string GenerateOrderNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        return $"ORD-{timestamp}-{random}";
    }
}