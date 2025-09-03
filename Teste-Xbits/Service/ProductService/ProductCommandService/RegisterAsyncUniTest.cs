using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductRequest;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Service.ProductService.ProductCommandService.Base;

namespace Teste_Xbits.Service.ProductService.ProductCommandService;

public class RegisterAsyncUniTest : ProductCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task RegisterAsync_ValidProduct_ReturnTrue()
    {
        var dtoRegister = CreateValidProductCreateRequest();
        var userCredential = new UserCredential 
        { 
            Id = Guid.NewGuid(), 
            Roles = [nameof(ERoles.Administrator)]
        };
        var product = CreateValidProduct();
        
        ProductRepository
            .Setup(x => x.FindByPredicateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(), null, false))
            .ReturnsAsync((Product)null!); 
        
        ProductRepository
            .Setup(x => x.SaveAsync(It.IsAny<Product>()))
            .ReturnsAsync(true);
            
        ProductMapper
            .Setup(x => x.DtoRegisterToDomain(It.IsAny<ProductRegisterRequest>()))
            .Returns(product);
            
        Validator
            .Setup(x => x.ValidationAsync(It.IsAny<Product>()))
            .ReturnsAsync(ValidationResponse);
        
        var result = await ProductCommandService.RegisterProductAsync(dtoRegister, userCredential);
        
        Assert.True(result);
        ProductRepository.Verify(x => x.FindByPredicateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(), null, false), Times.Once);
        ProductMapper.Verify(x => x.DtoRegisterToDomain(dtoRegister), Times.Once);
        Validator.Verify(x => x.ValidationAsync(product), Times.Once);
        ProductRepository.Verify(x => x.SaveAsync(product), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }
    
    [Fact]
    [Trait("Command", "Invalid Product")]
    public async Task RegisterAsync_InvalidProduct_ReturnFalse()
    {
        var dtoRegister = CreateInvalidProductCreateRequest();
        var userCredential = new UserCredential 
        { 
            Id = Guid.NewGuid(), 
            Roles = [nameof(ERoles.Administrator)]
        };
        
        CreateNotification();
        ProductRepository
            .Setup(x => x.FindByPredicateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(), null, false))
            .ReturnsAsync((Product)null!);
        
        var result = await ProductCommandService.RegisterProductAsync(dtoRegister, userCredential);
        
        Assert.False(result);
        NotificationHandler.Verify(x => x.CreateNotification(
            ProductTrace.Save,
            EMessage.Required.GetDescription().FormatTo("Nome")), Times.Once);
        ProductRepository.Verify(x => x.SaveAsync(It.IsAny<Product>()), Times.Never);
    }
    
    [Fact]
    [Trait("Command", "Save Fail")]
    public async Task RegisterAsync_SaveFail_ReturnFalse()
    {
        var dtoRegister = CreateValidProductCreateRequest();
        var userCredential = new UserCredential 
        { 
            Id = Guid.NewGuid(), 
            Roles = [nameof(ERoles.Administrator)]
        };
        var product = CreateValidProduct();
        
        ProductRepository
            .Setup(x => x.FindByPredicateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(), null, false))
            .ReturnsAsync((Product)null!);
            
        ProductRepository
            .Setup(x => x.SaveAsync(It.IsAny<Product>()))
            .ReturnsAsync(false);
            
        ProductMapper
            .Setup(x => x.DtoRegisterToDomain(It.IsAny<ProductRegisterRequest>()))
            .Returns(product);
            
        Validator
            .Setup(x => x.ValidationAsync(It.IsAny<Product>()))
            .ReturnsAsync(ValidationResponse);
        
        var result = await ProductCommandService.RegisterProductAsync(dtoRegister, userCredential);
        
        Assert.False(result);
        ProductRepository.Verify(x => x.SaveAsync(product), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}