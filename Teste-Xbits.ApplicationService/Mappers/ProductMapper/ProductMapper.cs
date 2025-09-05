using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Mappers.ProductMapper;

public class ProductMapper(IProductCategoryMapper categoryMapper) : IProductMapper
{
    public Product DtoRegisterToDomain(ProductRegisterRequest dtoRegister) =>
        new()
        {
            Name = dtoRegister.Name,
            Description = dtoRegister.Description,
            Price = dtoRegister.Price,
            Code = dtoRegister.Code,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HasExpirationDate = dtoRegister.HasExpirationDate,
            ExpirationDate = dtoRegister.ExpirationDate,
            ProductCategoryId = dtoRegister.ProductCategoryId == 0 
                ? null 
                : dtoRegister.ProductCategoryId,
        };

    public Product DtoUpdateToDomain(ProductUpdateRequest dtoUpdate, long productCategoryId) =>
        new()
        {
            Id = productCategoryId,
            Name = dtoUpdate.Name,
            Description = dtoUpdate.Description,
            Price = dtoUpdate.Price,
            Code = dtoUpdate.Code,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HasExpirationDate = dtoUpdate.HasExpirationDate,
            ExpirationDate = dtoUpdate.ExpirationDate,
            ProductCategoryId = dtoUpdate.ProductId == 0 
                ? null 
                : dtoUpdate.ProductCategoryId,
        };

    public ProductResponse DomainToSimpleResponse(Product product) =>
        new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Code = product.Code,
            HasExpirationDate = product.HasExpirationDate,
            ExpirationDate = product.ExpirationDate,
            ProductCategoryId = product.ProductCategoryId,
            ProductCategory = product.ProductCategory == null 
                ? null 
                : categoryMapper.DomainToSimpleResponse(product.ProductCategory), 
        };

    public PageList<ProductResponse> DomainToPaginationResponse(PageList<Product> productPageList)
    {
        var responses = productPageList.Items.Select(DomainToSimpleResponse).ToList();

        return new PageList<ProductResponse>(
            responses,
            productPageList.TotalCount,
            productPageList.CurrentPage,
            productPageList.PageSize);
    }
}