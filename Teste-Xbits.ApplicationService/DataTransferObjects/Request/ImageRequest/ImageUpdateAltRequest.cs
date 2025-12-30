namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;

public record ImageUpdateAltRequest
{
    public required long ImageId { get; init; }
    public required string Alt { get; init; }
}