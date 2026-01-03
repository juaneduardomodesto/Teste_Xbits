using Teste_Xbits.Domain.Entities.Base;

namespace Teste_Xbits.Domain.Entities;

public record CartItem : BaseEntity
{
    public long Id { get; init; }
    public required long CartId { get; init; }
    public required long ProductId { get; init; }
    public required int Quantity { get; init; }
    public required decimal UnitPrice { get; init; }
    public decimal TotalPrice => Quantity * UnitPrice;
    public virtual Cart? Cart { get; init; }
    public virtual Product? Product { get; init; }
}