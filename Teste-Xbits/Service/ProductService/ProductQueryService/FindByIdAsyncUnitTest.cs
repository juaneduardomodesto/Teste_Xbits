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
    public async Task FindById_PerfectSetting_ReturnProduct()
    {
        var productId = 1L;

        var product = CreateValidProduct();
        var productResponse = CreateValidProductResponse();
        
        ProductRepository
            .Setup(r => r.FindByPredicateAsync(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(product);
        
        ProductMapper
            .Setup(m => m.DomainToSimpleResponse(It.IsAny<Product>()))
            .Returns(productResponse);
        
        var result = await ProductQueryService.FindByIdAsync(productId);
        
        Assert.NotNull(result);
        
        ProductRepository.Verify(r => r.FindByPredicateAsync(
            It.IsAny<Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>?>(),
            It.IsAny<bool>()), Times.Once);
        
        ProductMapper.Verify(m => m.DomainToSimpleResponse(It.IsAny<Product>()), Times.Once);
    }
    
    [Fact]
    [Trait("Query", "Invalid Product")]
    public async Task FindById_InvalidProduct_ReturnsNull()
    {
        var productId = 999L;

        ProductRepository
            .Setup(r => r.FindByPredicateAsync(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync((Product?)null);
        
        var result = await ProductQueryService.FindByIdAsync(productId);
        
        Assert.Null(result);

        ProductRepository.Verify(r => r.FindByPredicateAsync(
            It.IsAny<Expression<Func<Product, bool>>>(),
            It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>?>(),
            It.IsAny<bool>()), Times.Once);

        ProductMapper.Verify(m => m.DomainToSimpleResponse(It.IsAny<Product>()), Times.Never);
    }

}