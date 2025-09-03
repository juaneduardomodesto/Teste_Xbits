using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Service.ProductService.ProductQueryService.Base;

namespace Teste_Xbits.Service.ProductService.ProductQueryService;

public class FindAllWithPaginationAsyncUniTest : ProductQueryServiceSetup
{
    [Fact]
    public async Task FindAllWithPaginationAsync_ValidProduct_ReturnsProductsPageList()
    {
        var productPageList = CreateProductPageList();
        var productResponsePageList = CreateProductResponsePageList();
        var filters = CreateExpirationDateFilter();

        SetupProductRepositoryFindAllWithPaginationAsync(productPageList);
        SetupProductMapperDomainToPaginationResponse(productResponsePageList);
        
        var result = await ProductQueryService.FindAllWithPaginationAsync(
            filters.namePrefix,
            filters.descriptionPrefix,
            filters.pricePrefix,
            filters.productCodePrefix,
            filters.hasValidadeDatePrefix,
            filters.expirationDate,
            filters.productCategoryIdPrefix,
            DefaultPageParams);
        
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
        
        ProductRepository.Verify(x => x.FindAllWithPaginationAsync(
            It.IsAny<PageParams>(),
            It.IsAny<Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()), Times.Once);
    }

    [Fact]
    public async Task FindAllWithPaginationAsync_EmptyList_ReturnsEmptyPageList()
    {
        var emptyPageList = new PageList<Product>([], 0, 1, 10);
        var filters = CreateNoFilters();

        SetupProductRepositoryFindAllWithPaginationAsync(emptyPageList);
        
        var result = await ProductQueryService.FindAllWithPaginationAsync(
            filters.namePrefix,
            filters.descriptionPrefix,
            filters.pricePrefix,
            filters.productCodePrefix,
            filters.hasValidadeDatePrefix,
            filters.expirationDate,
            filters.productCategoryIdPrefix,
            DefaultPageParams);
        
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
        
        ProductMapper.Verify(x => x.DomainToPaginationResponse(It.IsAny<PageList<Product>>()), Times.Never);
    }

    [Fact]
    public async Task FindAllWithPaginationAsync_WithInvalidExpirationDateFormat_StillCallsRepository()
    {
        var productPageList = CreateProductPageList();
        var productResponsePageList = CreateProductResponsePageList();
        (string?, string?, decimal?, string?, bool?, string?, long?) filters = 
            (null, null, null, null, null, "invalid-date", null);

        SetupProductRepositoryFindAllWithPaginationAsync(productPageList);
        SetupProductMapperDomainToPaginationResponse(productResponsePageList);
        
        var result = await ProductQueryService.FindAllWithPaginationAsync(
            filters.Item1,
            filters.Item2,
            filters.Item3,
            filters.Item4,
            filters.Item5,
            filters.Item6,
            filters.Item7,
            DefaultPageParams);
        
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);
        
        ProductRepository.Verify(x => x.FindAllWithPaginationAsync(
            It.IsAny<PageParams>(),
            It.IsAny<Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()), Times.Once);
    }
}