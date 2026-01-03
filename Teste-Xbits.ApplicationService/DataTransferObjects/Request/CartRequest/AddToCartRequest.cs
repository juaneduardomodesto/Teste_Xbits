namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.CartRequest;

public record AddToCartRequest
{
    public required long ProductId { get; init; }
    public required int Quantity { get; init; }
}