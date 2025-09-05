using System.Linq.Expressions;
using Moq;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Service.UserService.UserQueryService.Base;

namespace Teste_Xbits.Service.UserService.UserQueryService;

public class FindByIdAsyncUnitTest : UserQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Perfect setting")]
    public async Task FindByIdAsync_ValidUser_ReturnUserResponse()
    {
        var userId = 1L;
        var user = CreateValidUser();
        var userResponse = CreateUserResponse();
        
        SetupUserRepositoryFindByPredicateAsync(user);
        SetupUserMapperDomainToSimpleResponse(user, userResponse);
        
        var result = await UserQueryService.FindByIdAsync(userId);
        
        Assert.NotNull(result);
        Assert.Equal(userResponse.Id, result.Id);
        Assert.Equal(userResponse.Name, result.Name);
        Assert.Equal(userResponse.Email, result.Email);
        UserRepository.Verify(
            x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            It.IsAny<bool>()), Times.Once);
        UserMapper.Verify(
            x => x.DomainToSimpleResponse(user), Times.Once);
    }
    
    [Fact]
    [Trait("Query", "Invalid User")]
    public async Task FindByIdAsync_InvalidUser_ReturnNull()
    {
        const long userId = 999L;
        User? user = null;
        
        SetupUserRepositoryFindByPredicateAsync(user!);
        
        var result = await UserQueryService.FindByIdAsync(userId);
        
        Assert.Null(result);
        UserRepository.Verify(
            x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            It.IsAny<bool>()), Times.Once);
        UserMapper.Verify(
            x => x.DomainToSimpleResponse(It.IsAny<User>()), Times.Never);
    }
}