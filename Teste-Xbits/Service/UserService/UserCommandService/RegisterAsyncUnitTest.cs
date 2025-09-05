using Moq;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Service.UserService.UserCommandService.Base;

namespace Teste_Xbits.Service.UserService.UserCommandService;

public class RegisterAsyncUnitTest : UserCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task RegisterAsync_ValidUser_ReturnTrue()
    {
        var user = CreateValidUser();
        var dtoRegister = CreateValidUserRegisterRequest();
        var userId = Guid.NewGuid();

        SetupUserMapperDtoRegisterToDomain(dtoRegister, user);
        SetupValidatorValidationAsync();
        SetupUserRepositorySaveAsync(user, true);
        SetupLoggerHandlerCreateLogger();
        
        var result = await UserCommandService.RegisterUserAsync(dtoRegister, userId, false);
        
        Assert.True(result);
        UserMapper.Verify(
            x => x.DtoRegisterToDomain(dtoRegister), Times.Once);
        Validator.Verify(
            x => x.ValidationAsync(It.IsAny<User>()), Times.Once);
        UserRepository.Verify(
            x => x.SaveAsync(It.IsAny<User>()), Times.Once);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }
    
    [Fact]
    [Trait("Command", "Invalid User")]
    public async Task RegisterAsync_InvalidUser_ReturnFalse()
    {
        var user = CreateValidUser();
        var dtoRegister = CreateInvalidUserRegisterRequest();
        var userId = Guid.NewGuid();

        CreateNotification();
        SetupUserMapperDtoRegisterToDomain(dtoRegister, user);
        SetupValidatorValidationAsync();
        
        var result = await UserCommandService.RegisterUserAsync(dtoRegister, userId, false);
        
        Assert.False(result);
        UserMapper.Verify(
            x => x.DtoRegisterToDomain(dtoRegister), Times.Once);
        Validator.Verify(
            x => x.ValidationAsync(It.IsAny<User>()), Times.Once);
        UserRepository.Verify(
            x => x.SaveAsync(It.IsAny<User>()), Times.Never);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
    
    [Fact]
    [Trait("Command", "Save Fail")]
    public async Task RegisterAsync_SaveFail_ReturnFalse()
    {
        var user = CreateValidUser();
        var dtoRegister = CreateValidUserRegisterRequest();
        var userId = Guid.NewGuid();
        
        SetupUserMapperDtoRegisterToDomain(dtoRegister, user);
        SetupValidatorValidationAsync();
        SetupUserRepositorySaveAsync(user, false);
        SetupLoggerHandlerCreateLogger();
        
        var result = await UserCommandService.RegisterUserAsync(dtoRegister, userId, false);
        
        Assert.False(result);
        UserMapper.Verify(
            x => x.DtoRegisterToDomain(dtoRegister), Times.Once);
        Validator.Verify(
            x => x.ValidationAsync(It.IsAny<User>()), Times.Once);
        UserRepository.Verify(
            x => x.SaveAsync(It.IsAny<User>()), Times.Once);
        LoggerHandler.Verify(
            x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}