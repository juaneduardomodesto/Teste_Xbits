namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;

public record ImageUpdateOrderRequest
{
    public required List<ImageOrderItem> Items { get; init; }
}

public record ImageOrderItem
{
    public required long ImageId { get; init; }
    public required int DisplayOrder { get; init; }
}