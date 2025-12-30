namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;

public record ImageUpdateRequest : ImageRegisterRequest
{
    public required long Id { get; init; }
}