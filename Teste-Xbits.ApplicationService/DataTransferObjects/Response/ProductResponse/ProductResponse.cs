namespace Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductResponse;

public class ProductResponse
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public required string Code { get; init; }
    public bool HasExpirationDate { get; init; }
    public DateTime? ExpirationDate { get; init; }
    public long? ProductCategoryId { get; init; }
    public ProductCategoryResponse.ProductCategoryResponse? ProductCategory { get; init; }
}