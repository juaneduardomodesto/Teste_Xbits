using System.Linq.Expressions;
using Moq;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Service.UserService.UserCommandService.Base;

namespace Teste_Xbits.Service.UserService.UserCommandService;

public class DeleteAsyncUnitTest : UserCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task DeleteAsync_ValidUser_ReturnTrue()
    {
        var user = CreateValidUser();
        var dtoDelete = CreateUserDeleteRequest();
        var userCredential = new UserCredential { Id = Guid.NewGuid(), Roles = [] };
        
        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);

        UserRepository.Setup(x => x.DeleteAsync(user))
            .ReturnsAsync(true);

        LoggerHandler.Setup(x => x.CreateLogger(It.IsAny<DomainLogger>()));
    
        var result = await UserCommandService.DeleteUserAsync(dtoDelete, userCredential);
    
        Assert.True(result);
    
        UserRepository.Verify(x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            It.IsAny<bool>()), Times.Once);
    
        UserRepository.Verify(x => x.DeleteAsync(user), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Once);
    }
    
[Fact]
    [Trait("Command", "Invalid User")]
    public async Task DeleteAsync_InvalidUser_ReturnFalse()
    {
        var dtoDelete = CreateUserDeleteRequest();
        var user = CreateValidUser();
        var userCredential = new UserCredential { Id = Guid.NewGuid(), Roles = [] };

        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);
        
        var result = await UserCommandService.DeleteUserAsync(dtoDelete, userCredential);
        
        Assert.False(result);
        
        UserRepository.Verify(x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            It.IsAny<bool>()), Times.Once);
        
        UserRepository.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
    
    [Fact]
    [Trait("Command", "Delete Fail")]
    public async Task DeleteAsync_DeleteFail_ReturnFalse()
    {
        var user = CreateValidUser();
        var dtoDelete = CreateUserDeleteRequest();
        var userCredential = new UserCredential { Id = Guid.NewGuid(), Roles = [] };

        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);

        UserRepository.Setup(x => x.DeleteAsync(user))
            .ReturnsAsync(false);

        var result = await UserCommandService.DeleteUserAsync(dtoDelete, userCredential);

        Assert.False(result);
        
        UserRepository.Verify(x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            It.IsAny<bool>()), Times.Once);
        
        UserRepository.Verify(x => x.DeleteAsync(user), Times.Once);
        LoggerHandler.Verify(x => x.CreateLogger(It.IsAny<DomainLogger>()), Times.Never);
    }
}