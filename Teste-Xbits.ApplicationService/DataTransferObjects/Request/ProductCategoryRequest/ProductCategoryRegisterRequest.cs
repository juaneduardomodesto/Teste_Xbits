namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;

public record ProductCategoryRegisterRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Code { get; init; }
}