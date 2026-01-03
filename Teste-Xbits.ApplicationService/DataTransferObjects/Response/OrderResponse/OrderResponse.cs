using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.OrderResponse;

public record OrderResponse
{
    public long Id { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public EOrderStatus Status { get; init; }
    public EPaymentMethod PaymentMethod { get; init; }
    public EPaymentStatus PaymentStatus { get; init; }
    public decimal Subtotal { get; init; }
    public decimal Discount { get; init; }
    public decimal ShippingCost { get; init; }
    public decimal Total { get; init; }
    public List<OrderItemResponse> Items { get; init; } = new();
    public DateTime CreatedAt { get; init; }
    public DateTime? PaidAt { get; init; }
}