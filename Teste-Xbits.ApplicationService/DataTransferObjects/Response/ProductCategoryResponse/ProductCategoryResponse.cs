namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductCategoryResponse;

public class ProductCategoryResponse
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string ProductCategoryCode { get; init; }
}