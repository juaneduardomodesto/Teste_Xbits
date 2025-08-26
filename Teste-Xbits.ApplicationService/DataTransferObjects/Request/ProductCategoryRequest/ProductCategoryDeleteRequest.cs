namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;

public record ProductCategoryDeleteRequest
{
    public required long Id {get; init; }
}