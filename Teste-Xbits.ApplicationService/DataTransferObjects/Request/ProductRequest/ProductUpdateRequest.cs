namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductRequest;

public class ProductUpdateRequest : ProductRegisterRequest
{
    public long? ProductId { get; init; }
}