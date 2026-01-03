using Teste_Xbits.Domain.Entities.Base;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.Domain.Entities;

public record Order : BaseEntity
{
    public long Id { get; init; }
    public required string OrderNumber { get; init; }
    public required Guid UserId { get; init; }
    public required long CartId { get; init; }
    public required EOrderStatus Status { get; init; }
    public required EPaymentMethod PaymentMethod { get; init; }
    public EPaymentStatus PaymentStatus { get; init; }
    public required decimal Subtotal { get; init; }
    public decimal Discount { get; init; }
    public decimal ShippingCost { get; init; }
    public required decimal Total { get; init; }
    public DateTime? PaidAt { get; init; }
    public DateTime? CancelledAt { get; init; }
    public string? CancellationReason { get; init; }
    public virtual User? User { get; init; }
    public virtual Cart? Cart { get; init; }
    public virtual ICollection<OrderItem> Items { get; init; } = new List<OrderItem>();
}