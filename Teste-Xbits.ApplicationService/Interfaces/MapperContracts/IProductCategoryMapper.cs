using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductCategoryResponse;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface IProductCategoryMapper
{
    ProductCategory DtoRegisterToDomain(ProductCategoryRegisterRequest dtoRegister);
    ProductCategory DtoUpdateToDomain(ProductCategoryUpdateRequest dtoUpdate, long productCategoryId);
    ProductCategoryResponse DomainToSimpleResponse(ProductCategory user);
    public PageList<ProductCategoryResponse> DomainToPaginationUserResponse(PageList<ProductCategory> userPageList);
}