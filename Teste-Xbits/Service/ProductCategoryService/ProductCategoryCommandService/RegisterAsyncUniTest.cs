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
        var userCredential = CreateUserCredential();
        
        SetupProductCategoryMapperDtoRegisterToDomain(productCategoryRequest, productCategory);
        SetupValidatorValidationAsync();
        SetupProductCategoryRepositorySaveAsync(productCategory, true);
        SetupLoggerHandlerCreateLogger();

        var result = await ProductCategoryCommandService.RegisterAsync(productCategoryRequest, userCredential);
        
        Assert.True(result);
        ProductCategoryMapper.Verify(
            x => x.DtoRegisterToDomain(productCategoryRequest), Times.Once);
        Validator.Verify(
            x => x.ValidationAsync(It.IsAny<ProductCategory>()), Times.Once);
        ProductCategoryRepository.Verify(
            x => x.SaveAsync(productCategory), Times.Once);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }
    
    [Fact]
    [Trait("Command", "invalide Product Category")]
    public async Task RegisterAsync_InvalidCategory_returnFalse()
    {
        var productCategory = CreateValidProductCategory();
        var productCategoryRequest = CreateValidProductCategoryRegisterRequest();
        var userCredential = CreateUserCredential();
        
        CreateNotification();
        SetupProductCategoryMapperDtoRegisterToDomain(productCategoryRequest, productCategory);
        SetupValidatorValidationAsync();
        SetupValidatorValidationAsync();

        var result = await ProductCategoryCommandService.RegisterAsync(productCategoryRequest, userCredential);
        
        Assert.False(result);
        ProductCategoryMapper.Verify(
            x => x.DtoRegisterToDomain(productCategoryRequest), Times.Once);
        Validator.Verify(
            x => x.ValidationAsync(It.IsAny<ProductCategory>()), Times.Once);
        ProductCategoryRepository.Verify(
            x => x.SaveAsync(productCategory), Times.Never);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
    
    [Fact]
    [Trait("Command", "save Fail")]
    public async Task RegisterAsync_SaveFail_returnFalse()
    {
        var productCategory = CreateValidProductCategory();
        var productCategoryRequest = CreateValidProductCategoryRegisterRequest();
        var userCredential = CreateUserCredential();
        
        SetupProductCategoryMapperDtoRegisterToDomain(productCategoryRequest, productCategory);
        SetupValidatorValidationAsync();
        SetupProductCategoryRepositorySaveAsync(productCategory, false);
        SetupLoggerHandlerCreateLogger();

        var result = await ProductCategoryCommandService.RegisterAsync(productCategoryRequest, userCredential);
        
        Assert.False(result);
        ProductCategoryMapper.Verify(
            x => x.DtoRegisterToDomain(productCategoryRequest), Times.Once);
        Validator.Verify(
            x => x.ValidationAsync(It.IsAny<ProductCategory>()), Times.Once);
        ProductCategoryRepository.Verify(
            x => x.SaveAsync(productCategory), Times.Once);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}