using Moq;
using Teste_Xbits.ApplicationService.Services.UserService;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Infra.Repositories;
using Teste_Xbits.Service.ProductCategoryService.ProductCategoryCommandService.Base;

namespace Teste_Xbits.Service.ProductCategoryService.ProductCategoryCommandService;

public class RegisterAsyncUniTest : ProductCategoryCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task RegisterAsync_ValidCategory_returnTrue()
    {
        var productCategory = CreateValidProductCategory();
        var productCategoryRequest = CreateValidProductCategoryRegisterRequest();
        var userCredential = new UserCredential 
        { 
            Id = Guid.NewGuid(), 
            Roles = [nameof(ERoles.Administrator)]
        };
        
        ProductCategoryMapper.Setup(
                x => x.DtoRegisterToDomain(productCategoryRequest))
            .Returns(productCategory);
        Validator.Setup(x => x.ValidationAsync(It.IsAny<ProductCategory>()))
            .ReturnsAsync(ValidationResponse);
        ProductCategoryRepository.Setup(x => x.SaveAsync(productCategory))
            .ReturnsAsync(true);
        LoggerHandler.Setup(x => x.CreateLogger(It.IsAny<DomainLogger>()));

        var result = await ProductCategoryCommandService.RegisterAsync(productCategoryRequest, userCredential);
        
        ProductCategoryMapper.Verify(x => x.DtoRegisterToDomain(productCategoryRequest), Times.Once);
        Validator.Verify(x => x.ValidationAsync(It.IsAny<ProductCategory>()), Times.Once);
        ProductCategoryRepository.Verify(x => x.SaveAsync(productCategory), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }
    
    [Fact]
    [Trait("Command", "invalide Product Category")]
    public async Task RegisterAsync_InvalidCategory_returnFalse()
    {
        var productCategory = CreateValidProductCategory();
        var productCategoryRequest = CreateValidProductCategoryRegisterRequest();
        var userCredential = new UserCredential 
        { 
            Id = Guid.NewGuid(), 
            Roles = [nameof(ERoles.Administrator)]
        };
        
        CreateNotification();
        ProductCategoryMapper.Setup(
                x => x.DtoRegisterToDomain(productCategoryRequest))
            .Returns(productCategory);
        Validator.Setup(x => x.ValidationAsync(It.IsAny<ProductCategory>()))
            .ReturnsAsync(ValidationResponse);

        var result = await ProductCategoryCommandService.RegisterAsync(productCategoryRequest, userCredential);
        
        ProductCategoryMapper.Verify(x => x.DtoRegisterToDomain(productCategoryRequest), Times.Once);
        Validator.Verify(x => x.ValidationAsync(It.IsAny<ProductCategory>()), Times.Once);
        ProductCategoryRepository.Verify(x => x.SaveAsync(productCategory), Times.Never);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
    
    [Fact]
    [Trait("Command", "save Fail")]
    public async Task RegisterAsync_SaveFail_returnFalse()
    {
        var productCategory = CreateValidProductCategory();
        var productCategoryRequest = CreateValidProductCategoryRegisterRequest();
        var userCredential = new UserCredential 
        { 
            Id = Guid.NewGuid(), 
            Roles = [nameof(ERoles.Administrator)]
        };
        
        ProductCategoryMapper.Setup(
                x => x.DtoRegisterToDomain(productCategoryRequest))
            .Returns(productCategory);
        Validator.Setup(x => x.ValidationAsync(It.IsAny<ProductCategory>()))
            .ReturnsAsync(ValidationResponse);
        ProductCategoryRepository.Setup(x => x.SaveAsync(productCategory))
            .ReturnsAsync(false);
        LoggerHandler.Setup(x => x.CreateLogger(It.IsAny<DomainLogger>()));

        var result = await ProductCategoryCommandService.RegisterAsync(productCategoryRequest, userCredential);
        
        ProductCategoryMapper.Verify(x => x.DtoRegisterToDomain(productCategoryRequest), Times.Once);
        Validator.Verify(x => x.ValidationAsync(It.IsAny<ProductCategory>()), Times.Once);
        ProductCategoryRepository.Verify(x => x.SaveAsync(productCategory), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}