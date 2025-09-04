using System.Linq.Expressions;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductCategoryResponse;
using Teste_Xbits.ApplicationService.Mappers.ProductMapper;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.Repositories;
using Teste_Xbits.Service.ProductCategoryService.ProductQueryService.Base;

namespace Teste_Xbits.Service.ProductCategoryService.ProductQueryService;

public class FindAllByIdAsyncUnitTest : ProductCategoryQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Perfect setting")]
    public async Task FindAllByIdAsync_ValidCategory_ReturnCategory()
    {
        var productCategory = CreateValidProductCategory();
        var productCategoryResponse = CreateValidProductCategoryResponse();
        const long productCategoryId = 1L;

        SetupProductCategoryRepositoryFindByPredicateAsync(productCategory);
        SetupProductCategoryMapperDomainToSimpleResponse(productCategoryResponse);
        
        var result = await ProductCategoryQueryService.FindByIdAsync(productCategoryId);
        
        Assert.NotNull(result);
        
        ProductCategoryRepository.Verify(r => r.FindByPredicateAsync(
            It.IsAny<Expression<Func<ProductCategory, bool>>>(),
            It.IsAny<Func<IQueryable<ProductCategory>, IQueryable<ProductCategory>>?>(),
            It.IsAny<bool>()), Times.Once);
        
        ProductCategoryMapper.Verify(m => m.DomainToSimpleResponse(It.IsAny<ProductCategory>()), Times.Once);
    }
    
    [Fact]
    [Trait("Query", "Empty list")]
    public async Task FindAllByIdAsync_InvalidProductCategory_ReturnNull()
    {
        const long productCategoryId = 1L;
        ProductCategory? productCategory = null;
        
        SetupProductCategoryRepositoryFindByPredicateAsync(productCategory!);
        
        var result = await ProductCategoryQueryService.FindByIdAsync(productCategoryId);
        
        Assert.Null(result);
        
        ProductCategoryRepository.Verify(r => r.FindByPredicateAsync(
            It.IsAny<Expression<Func<ProductCategory, bool>>>(),
            It.IsAny<Func<IQueryable<ProductCategory>, IQueryable<ProductCategory>>?>(),
            It.IsAny<bool>()), Times.Once);
        
        ProductCategoryMapper.Verify(m => m.DomainToSimpleResponse(It.IsAny<ProductCategory>()), Times.Never);
    }
}