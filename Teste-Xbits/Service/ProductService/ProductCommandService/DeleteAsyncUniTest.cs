using Moq;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Service.ProductService.ProductCommandService.Base;

namespace Teste_Xbits.Service.ProductService.ProductCommandService;

public class DeleteAsyncUniTest : ProductCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task DeleteProductAsync_ValidProduct_ReturnTrue()
    {
        var dtoDelete = CreateProductDeleteRequest();
        var userCredential = new UserCredential { Id = Guid.NewGuid(), Roles = [nameof(ERoles.Administrator)] };
        var existingProduct = CreateValidProduct();

        ProductRepository
            .Setup(x => x.FindByPredicateAsync(
                x => x.Id == dtoDelete.ProductId, null, false))
            .ReturnsAsync(existingProduct);

        ProductRepository
            .Setup(x => x.DeleteAsync(existingProduct))
            .ReturnsAsync(true);
        
        var result = await ProductCommandService.DeleteProductAsync(dtoDelete, userCredential);
        
        Assert.True(result);
        ProductRepository.Verify(x => x.FindByPredicateAsync(
            x => x.Id == dtoDelete.ProductId, null, false), Times.Once);
        ProductRepository.Verify(x => x.DeleteAsync(existingProduct), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }
    
    [Fact]
    [Trait("Command", "Invalid Product")]
    public async Task DeleteProductAsync_InvalidProduct_ReturnFalse()
    {
        var dtoDelete = CreateInvalideProductDeleteRequest();
        var userCredential = new UserCredential { Id = Guid.NewGuid(), Roles = [nameof(ERoles.Administrator)] };
        
        CreateNotification();
        
        var result = await ProductCommandService.DeleteProductAsync(dtoDelete, userCredential);
        
        Assert.False(result);
        NotificationHandler.Verify(x => x.CreateNotification(
            ProductTrace.Delete,
            EMessage.ProductNotFound.GetDescription()), Times.Once);
        ProductRepository.Verify(x => x.DeleteAsync(It.IsAny<Product>()), Times.Never);
    }
    
    [Fact]
    [Trait("Command", "Delete Fail")]
    public async Task DeleteProductAsync_DeleteFail_ReturnFalse()
    {
        var dtoDelete = CreateProductDeleteRequest();
        var userCredential = new UserCredential { Id = Guid.NewGuid(), Roles = [nameof(ERoles.Administrator)] };
        var existingProduct = CreateValidProduct();

        ProductRepository
            .Setup(x => x.FindByPredicateAsync(
                x => x.Id == dtoDelete.ProductId, null, false))
            .ReturnsAsync(existingProduct);

        ProductRepository
            .Setup(x => x.DeleteAsync(existingProduct))
            .ReturnsAsync(false);
        
        var result = await ProductCommandService.DeleteProductAsync(dtoDelete, userCredential);
        
        Assert.False(result);
        ProductRepository.Verify(x => x.FindByPredicateAsync(
            x => x.Id == dtoDelete.ProductId, null, false), Times.Once);
        ProductRepository.Verify(x => x.DeleteAsync(existingProduct), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}