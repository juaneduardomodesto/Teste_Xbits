using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Service.ProductService.ProductQueryService.Base;

namespace Teste_Xbits.Service.ProductService.ProductQueryService;

public class FindByIdAsyncUnitTest : ProductQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Perfect setting")]
    public async Task FindById_ValidProduct_ReturnProduct()
    {
        const long productId = 1L;
        var product = CreateValidProduct();
        var productResponse = CreateValidProductResponse();
        
        SetupProductRepositoryFindByPredicateAsync(product);
        SetupProductMapperDomainToSimpleResponse(productResponse);
        
        var result = await ProductQueryService.FindByIdAsync(productId);
        
        Assert.NotNull(result);
        ProductRepository.Verify(
            r => r.FindByPredicateAsync(
            It.IsAny<Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>?>(),
            It.IsAny<bool>()), Times.Once);
        
        ProductMapper.Verify(
            m => m.DomainToSimpleResponse(It.IsAny<Product>()), Times.Once);
    }
    
    [Fact]
    [Trait("Query", "Invalid Product")]
    public async Task FindById_InvalidProduct_ReturnsNull()
    {
        const long productId = 999L;
        Product? product = null;

        SetupProductRepositoryFindByPredicateAsync(product!);
        
        var result = await ProductQueryService.FindByIdAsync(productId);
        
        Assert.Null(result);
        ProductRepository.Verify(
            r => r.FindByPredicateAsync(
            It.IsAny<Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>?>(),
            It.IsAny<bool>()), Times.Once);
        ProductMapper.Verify(
            m => m.DomainToSimpleResponse(It.IsAny<Product>()), Times.Never);
    }
}