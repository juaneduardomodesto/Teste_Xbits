using Teste_Xbits.Domain.Entities.Base;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.Domain.Entities;

public record Cart : BaseEntity
{
    public long Id { get; init; }
    public required long UserId { get; init; }
    public required ECartStatus Status { get; init; }
    public DateTime? CheckedOutAt { get; init; }
    public virtual User? User { get; init; }
    public virtual ICollection<CartItem> Items { get; init; } = new List<CartItem>();
    public decimal Subtotal => Items.Sum(i => i.TotalPrice);
    public int TotalItems => Items.Sum(i => i.Quantity);
}