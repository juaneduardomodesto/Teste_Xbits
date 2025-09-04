using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductCategoryResponse;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Service.ProductCategoryService.ProductQueryService.Base;

namespace Teste_Xbits.Service.ProductCategoryService.ProductQueryService;

public class FindAllWithPaginationAsyncUnitTest : ProductCategoryQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Perfect setting")]
    public async Task FindAllWithPaginationAsync_ValidCategory_ReturnProductCategoryPageList()
    {
        var productCategoryPageList = CreateProductPageList();
        var productCategoryResponsePageList = CreateProductCategoryResponsePageList();
        var pageParam = new PageParams();

        SetupProductCategoryRepositoryFindAllWithPaginationAsync(productCategoryPageList);
        SetupProductCategoryMapperDomainToPaginationResponse(productCategoryResponsePageList);

        var result = 
            await ProductCategoryQueryService.FindAllWithPaginationAsync(
                null,null, null, pageParam);
        
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
        ProductCategoryRepository.Verify(x => x.FindAllWithPaginationAsync(
            It.IsAny<PageParams>(),
            It.IsAny<Expression<Func<ProductCategory, bool>>>(),
            It.IsAny<Func<IQueryable<ProductCategory>, 
                IIncludableQueryable<ProductCategory, object>>>()), Times.Once);
        ProductCategoryMapper.Verify(x => x.DomainToPaginationResponse(
            It.IsAny<PageList<ProductCategory>>()), Times.Once);
    }
    
    [Fact]
    [Trait("Query", "Empty List")]
    public async Task FindAllWithPaginationAsync_InvalidCategory_ReturnEmptyPageList()
    {
        var emptyProductCategoryPageList = new PageList<ProductCategory>(
            [], 0, 1, 10);
        var emptyProductCategoryResponsePageList = new PageList<ProductCategoryResponse>(
            [], 0, 1, 10);
        var pageParam = new PageParams { PageNumber = 1, PageSize = 10 };
        
        SetupProductCategoryRepositoryFindAllWithPaginationAsync(emptyProductCategoryPageList);
        SetupProductCategoryMapperDomainToPaginationResponse(emptyProductCategoryResponsePageList);
        
        var result = await ProductCategoryQueryService.FindAllWithPaginationAsync(
            null, null, null, pageParam);
        
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        ProductCategoryRepository.Verify(x => x.FindAllWithPaginationAsync(
            It.IsAny<PageParams>(),
            It.IsAny<Expression<Func<ProductCategory, bool>>>(),
            It.IsAny<Func<IQueryable<ProductCategory>, 
                IIncludableQueryable<ProductCategory, object>>>()), Times.Once);
        ProductCategoryMapper.Verify(x => x.DomainToPaginationResponse(
            It.IsAny<PageList<ProductCategory>>()), Times.Never);
    }
}