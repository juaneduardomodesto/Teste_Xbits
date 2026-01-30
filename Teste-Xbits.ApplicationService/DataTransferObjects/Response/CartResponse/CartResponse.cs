using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.CartResponse;

public record CartResponse
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public ECartStatus Status { get; init; }
    public decimal Subtotal { get; init; }
    public int TotalItems { get; init; }
    public List<CartItemResponse> Items { get; init; } = new();
    public DateTime CreatedAt { get; init; }
}