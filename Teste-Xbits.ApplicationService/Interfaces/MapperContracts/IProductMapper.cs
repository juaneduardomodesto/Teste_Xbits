using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductResponse;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface IProductMapper
{
    Product DtoRegisterToDomain(ProductRegisterRequest dtoRegister);
    Product DtoUpdateToDomain(ProductUpdateRequest dtoUpdate, long productCategoryId);
    ProductResponse DomainToSimpleResponse(Product product);
    PageList<ProductResponse> DomainToPaginationResponse(PageList<Product> productPageList);
}