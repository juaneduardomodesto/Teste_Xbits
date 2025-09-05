using System.Linq.Expressions;
using Moq;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Service.ProductCategoryService.ProductCategoryCommandService.Base;

namespace Teste_Xbits.Service.ProductCategoryService.ProductCategoryCommandService;

public class DeleteAsyncUnitTest : ProductCategoryCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task DeleteAsync_ValidProductCategory_ReturnTrue()
    {
        var productCategoryRequest = CreateValidProductCategoryDeleteRequest();
        var userCredential = CreateUserCredential();
        var existingProductCategory = CreateValidProductCategory();
        
        SetupProductCategoryFindPredicateAsync(existingProductCategory);
        SetupProductCategoryRepositoryDeleteAsync(existingProductCategory, true);

        var result = 
            await ProductCategoryCommandService.DeleteRegisterAsync(productCategoryRequest, userCredential);

        Assert.True(result);
        ProductCategoryRepository.Verify(
            x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<ProductCategory, bool>>>(), null, false), Times.Once);
        ProductCategoryRepository.Verify(
            x => x.DeleteAsync(existingProductCategory), Times.Once);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }
    
    [Fact]
    [Trait("Command", "Invalid Product Category")]
    public async Task DeleteAsync_InvalidProductCategory_ReturnFalse()
    {
        var productCategoryRequest = CreateValidProductCategoryDeleteRequest();
        var userCredential = CreateUserCredential();
        ProductCategory? existingProductCategory = null;

        CreateNotification();
        SetupProductCategoryFindPredicateAsync(existingProductCategory!);

        var result = await ProductCategoryCommandService.DeleteRegisterAsync(productCategoryRequest, userCredential);

        Assert.False(result);
        ProductCategoryRepository.Verify(
            x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<ProductCategory, bool>>>(), null, false), Times.Once);
        ProductCategoryRepository.Verify(
            x => x.DeleteAsync(existingProductCategory!), Times.Never);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
    
    [Fact]
    [Trait("Command", "Delete Fail")]
    public async Task DeleteAsync_DeleteFail_ReturnFalse()
    {
        var dtoDelete = CreateValidProductCategoryDeleteRequest();
        var userCredential = CreateUserCredential();
        var existingProductCategory = CreateValidProductCategory();

        SetupProductCategoryFindPredicateAsync(existingProductCategory);
        SetupProductCategoryRepositoryDeleteAsync(existingProductCategory, false);

        var result = await ProductCategoryCommandService.DeleteRegisterAsync(dtoDelete, userCredential);

        Assert.False(result);
        ProductCategoryRepository.Verify(
            x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<ProductCategory, bool>>>(), null, false), Times.Once);
        ProductCategoryRepository.Verify(
            x => x.DeleteAsync(existingProductCategory), Times.Once);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}