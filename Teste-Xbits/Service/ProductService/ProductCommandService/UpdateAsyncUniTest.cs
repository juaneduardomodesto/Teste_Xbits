using Moq;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Service.ProductService.ProductCommandService.Base;

namespace Teste_Xbits.Service.ProductService.ProductCommandService;

public class UpdateAsyncUniTest : ProductCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task UpdateAsync_ValidProduct_ReturnTrue()
    {
        var dtoUpdate = CreateValidProductUpdateRequest();
        var userCredential = new UserCredential 
        { 
            Id = Guid.NewGuid(), 
            Roles = [nameof(ERoles.Administrator)]
        };
        var existingProduct = CreateValidProduct();
        var updatedProduct = CreateValidProduct();

        SetupProductRepositoryFindByPredicateAsyncPreExist();
        SetupProductRepositoryFindByPredicateAsync(dtoUpdate, existingProduct, true);
        SetupProductMapperDtoUpdateToDomain(dtoUpdate, existingProduct, updatedProduct);
        SetupValidationAsync(updatedProduct);
        SetupProductRepositoryUpdateAsync(updatedProduct, true);
        
        var result = await ProductCommandService.UpdateProductAsync(dtoUpdate, userCredential);
        
        Assert.True(result);
        ProductRepository.Verify(
            x => x.FindByPredicateAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(), null, false), Times.AtLeastOnce);
        ProductRepository.Verify(
            x => x.FindByPredicateAsync(
            x => x.Id == dtoUpdate.ProductId, null, true), Times.Once);
        ProductMapper.Verify(
            x => x.DtoUpdateToDomain(dtoUpdate, existingProduct.Id), Times.Once);
        Validator.Verify(
            x => x.ValidationAsync(updatedProduct), Times.Once);
        ProductRepository.Verify(
            x => x.UpdateAsync(updatedProduct), Times.Once);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }
    
    [Fact]
    [Trait("Command", "Invalid Product")]
    public async Task UpdateAsync_WithNonExistentCategory_ReturnFalse()
    {
        var dtoUpdate = CreateInvalidProductUpdateRequest();
        var userCredential = new UserCredential 
        { 
            Id = Guid.NewGuid(), 
            Roles = [nameof(ERoles.Administrator)]
        };

        CreateNotification();        
        SetupProductRepositoryFindByPredicateAsyncPreExist();
        SetupCategoryRepositoryFindByPredicateAsync();

        var result = await ProductCommandService.UpdateProductAsync(dtoUpdate, userCredential);
        
        Assert.False(result);
        NotificationHandler.Verify(x => x.CreateNotification(
            ProductTrace.Update,
            EMessage.Required.GetDescription().FormatTo("Código do Produto")), Times.Once);
        ProductRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }
    
    [Fact]
    [Trait("Command", "Update Fail")]
    public async Task UpdateAsync_UpdateFail_ReturnFalse()
    {
        var dtoUpdate = CreateValidProductUpdateRequest();
        var userCredential = new UserCredential 
        { 
            Id = Guid.NewGuid(), 
            Roles = [nameof(ERoles.Administrator)]
        };
        var existingProduct = CreateValidProduct();
        var updatedProduct = CreateValidProduct();

        SetupProductRepositoryFindByPredicateAsyncPreExist();
        SetupProductRepositoryFindByPredicateAsync(dtoUpdate, existingProduct, true);
        SetupProductMapperDtoUpdateToDomain(dtoUpdate, existingProduct, updatedProduct);
        SetupValidationAsync(updatedProduct);
        SetupProductRepositoryUpdateAsync(updatedProduct, false);
        
        var result = await ProductCommandService.UpdateProductAsync(dtoUpdate, userCredential);
        
        Assert.False(result);
        ProductRepository.Verify(
            x => x.FindByPredicateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(), null, false), Times.AtLeastOnce);
        ProductRepository.Verify(
            x => x.FindByPredicateAsync(
                x => x.Id == dtoUpdate.ProductId, null, true), Times.Once);
        ProductMapper.Verify(
            x => x.DtoUpdateToDomain(dtoUpdate, existingProduct.Id), Times.Once);
        Validator.Verify(
            x => x.ValidationAsync(updatedProduct), Times.Once);
        ProductRepository.Verify(
            x => x.UpdateAsync(updatedProduct), Times.Once);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}