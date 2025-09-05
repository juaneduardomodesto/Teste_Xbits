using System.Linq.Expressions;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Service.ProductCategoryService.ProductCategoryCommandService.Base;

namespace Teste_Xbits.Service.ProductCategoryService.ProductCategoryCommandService;

public class UpdateAsyncUnitTest : ProductCategoryCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task UpdateRegisterAsync_ValidProductCategory_ReturnTrue()
    {
        var dtoUpdate = CreateValidProductCategoryUpdateRequest();
        var userCredential = CreateUserCredential();
        var existingProductCategory = CreateValidProductCategory();
        var updatedProductCategory = CreateValidProductCategory();

        SetupProductCategoryRepositoryFindByPredicateAsync(dtoUpdate, existingProductCategory);
        SetupProductCategoryMapperDtoUpdateToDomain(dtoUpdate, existingProductCategory, updatedProductCategory);
        SetupValidatorValidationAsync();
        SetupProductCategoryRepositoryUpdateAsync(updatedProductCategory, true);
        
        var result = await ProductCategoryCommandService.UpdateRegisterAsync(dtoUpdate, userCredential);
        
        Assert.True(result);
        ProductCategoryRepository.Verify(
            x => x.FindByPredicateAsync(
                x => x.Id == dtoUpdate.Id, null, true), Times.Once);
        ProductCategoryMapper.Verify(
            x => x.DtoUpdateToDomain(dtoUpdate, existingProductCategory.Id), Times.Once);
        Validator.Verify(
            x => x.ValidationAsync(updatedProductCategory), Times.Once);
        ProductCategoryRepository.Verify(
            x => x.UpdateAsync(updatedProductCategory), Times.Once);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }

    [Fact]
    [Trait("Command", "Product Category Not Found")]
    public async Task UpdateRegisterAsync_ProductCategoryNotFound_ReturnFalse()
    {
        var dtoUpdate = CreateValidProductCategoryUpdateRequest();
        var userCredential = CreateUserCredential();
        
        SetupProductCategoryRepositoryFindByPredicateAsyncFindNull(dtoUpdate);
        
        var result = await ProductCategoryCommandService.UpdateRegisterAsync(dtoUpdate, userCredential);
        
        Assert.False(result);
        ProductCategoryRepository.Verify(
            x => x.FindByPredicateAsync(
                x => x.Id == dtoUpdate.Id, null, true), Times.Once);
        ProductCategoryMapper.Verify(
            x => x.DtoUpdateToDomain(It.IsAny<ProductCategoryUpdateRequest>(), 
                It.IsAny<long>()), Times.Never);
        Validator.Verify(
            x => x.ValidationAsync(It.IsAny<ProductCategory>()), Times.Never);
        ProductCategoryRepository.Verify(
            x => x.UpdateAsync(It.IsAny<ProductCategory>()), Times.Never);
    }

    [Fact]
    [Trait("Command", "Null Name")]
    public async Task UpdateRegisterAsync_NullName_ReturnFalse()
    {
        var dtoUpdate = new ProductCategoryUpdateRequest
        {
            Id = 1,
            Name = null!,
            Description = "Updated Description",
            Code = "UPD001"
        };
        var userCredential = CreateUserCredential();
        
        var result = await ProductCategoryCommandService.UpdateRegisterAsync(dtoUpdate, userCredential);
        
        Assert.False(result);
        ProductCategoryRepository.Verify(
            x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<ProductCategory, bool>>>(), null, true), Times.Never);
    }
}