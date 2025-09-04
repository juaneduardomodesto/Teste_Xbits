using System.Linq.Expressions;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Service.UserService.UserCommandService.Base;

namespace Teste_Xbits.Service.UserService.UserCommandService;

public class UpdateAsyncUnitTest : UserCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task UpdateAsync_ValidUser_ReturnTrue()
    {
        var dtoUpdate = CreateValidUserUpdateRequest();
        var user = CreateValidUser();
        var updatedUser = CreateValidUser();
        var userId = Guid.NewGuid();

        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                null, true))
            .ReturnsAsync((user));
        UserMapper.Setup(x => x.DtoUpdateToDomain(dtoUpdate, 1L)).Returns(updatedUser);
        Validator.Setup(x => x.ValidationAsync(It.IsAny<User>())).ReturnsAsync(ValidationResponse);
        UserRepository.Setup(x => x.UpdateAsync(updatedUser)).ReturnsAsync(true);
        LoggerHandler.Setup(x => x.CreateLogger(It.IsAny<DomainLogger>()));

        var userCredential = new UserCredential { Id = userId, Roles = [] };
        var result = await UserCommandService.UpdateUserAsync(dtoUpdate, userCredential);
        
        Assert.True(result);
        UserMapper.Verify(x => x.DtoUpdateToDomain(dtoUpdate, 1L), Times.Once);
        Validator.Verify(x => x.ValidationAsync(It.IsAny<User>()), Times.Once);
        UserRepository.Verify(x => x.UpdateAsync(updatedUser), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }

    [Fact]
    [Trait("Command", "Invalid User")]
    public async Task UpdateAsync_InvalidUser_ReturnFalse()
    {
        var dtoUpdate = CreateValidUserUpdateRequest();
        var userId = Guid.NewGuid();
        
        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                null, true))
            .ReturnsAsync((User)null!);
    
        var userCredential = new UserCredential { Id = userId, Roles = [] };
        var result = await UserCommandService.UpdateUserAsync(dtoUpdate, userCredential);
    
        Assert.False(result);
        
        UserMapper.Verify(x => x.DtoUpdateToDomain(It.IsAny<UserUpdateRequest>(), It.IsAny<long>()), Times.Never);
        Validator.Verify(x => x.ValidationAsync(It.IsAny<User>()), Times.Never);
        UserRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
    
    [Fact]
    [Trait("Command", "Save Fail")]
    public async Task UpdateAsync_UpdateFail_ReturnFalse()
    {
        var dtoUpdate = CreateValidUserUpdateRequest();
        var user = CreateValidUser();
        var updatedUser = CreateValidUser();
        var userId = Guid.NewGuid();
        
        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                null, true))
            .ReturnsAsync(user);
        
        UserMapper.Setup(x => x.DtoUpdateToDomain(dtoUpdate, user.Id))
            .Returns(updatedUser);
        
        Validator.Setup(x => x.ValidationAsync(It.IsAny<User>()))
            .ReturnsAsync(ValidationResponse);
        
        UserRepository.Setup(x => x.UpdateAsync(updatedUser))
            .ReturnsAsync(false);
    
        var userCredential = new UserCredential { Id = userId, Roles = [] };
        var result = await UserCommandService.UpdateUserAsync(dtoUpdate, userCredential);
        
        Assert.False(result);
        
        UserRepository.Verify(x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            null, true), Times.Once);
    
        UserMapper.Verify(x => x.DtoUpdateToDomain(dtoUpdate, user.Id), Times.Once);
        Validator.Verify(x => x.ValidationAsync(updatedUser), Times.Once);
        UserRepository.Verify(x => x.UpdateAsync(updatedUser), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}