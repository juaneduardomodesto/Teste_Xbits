namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.CartRequest;

public record UpdateCartItemRequest
{
    public required long CartItemId { get; init; }
    public required int Quantity { get; init; }
}