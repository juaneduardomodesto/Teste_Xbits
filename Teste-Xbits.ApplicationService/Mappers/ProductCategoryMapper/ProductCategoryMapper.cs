using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductCategoryResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Mappers.ProductCategoryMapper;

public class ProductCategoryMapper : IProductCategoryMapper
{
    public ProductCategory DtoRegisterToDomain(ProductCategoryRegisterRequest productCategoryRegister) =>
        new()
        {
            Name = productCategoryRegister.Name!,
            Description = productCategoryRegister.Description!,
            ProductCategoryCode = productCategoryRegister.Code!,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

    public ProductCategory DtoUpdateToDomain(ProductCategoryUpdateRequest dtoUpdate, long productCategoryId) =>
        new()
        {
            Id = productCategoryId,
            Name = dtoUpdate.Name!,
            Description = dtoUpdate.Description!,
            ProductCategoryCode = dtoUpdate.Code!,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

    public ProductCategoryResponse DomainToSimpleResponse(ProductCategory user) =>
        new()
        {
            Id = user.Id!,
            Name = user.Name!,
            Description = user.Description!,
            ProductCategoryCode = user.ProductCategoryCode,
        };

    public PageList<ProductCategoryResponse> DomainToPaginationUserResponse(PageList<ProductCategory> userPageList)
    {
        var responses = userPageList.Items.Select(DomainToSimpleResponse).ToList();
        
        return new PageList<ProductCategoryResponse>(
            responses,
            userPageList.TotalCount,
            userPageList.CurrentPage,
            userPageList.PageSize);
    }
}