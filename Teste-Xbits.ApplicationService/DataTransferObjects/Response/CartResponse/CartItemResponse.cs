namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.CartResponse;

public record CartItemResponse
{
    public long Id { get; init; }
    public long ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string ProductCode { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalPrice { get; init; }
    public string? ProductImageUrl { get; init; }
}