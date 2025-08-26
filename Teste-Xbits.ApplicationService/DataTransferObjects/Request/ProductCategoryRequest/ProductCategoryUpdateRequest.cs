namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;

public record ProductCategoryUpdateRequest : ProductCategoryRegisterRequest
{
    public required long Id {get; init; }
}