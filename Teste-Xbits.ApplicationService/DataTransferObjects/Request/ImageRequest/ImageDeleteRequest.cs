namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;

public record ImageDeleteRequest
{
    public required long Id { get; init; }
}