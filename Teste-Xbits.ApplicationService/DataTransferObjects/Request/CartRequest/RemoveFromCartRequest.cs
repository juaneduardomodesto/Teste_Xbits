namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.CartRequest;

public record RemoveFromCartRequest
{
    public required long CartItemId { get; init; }
}