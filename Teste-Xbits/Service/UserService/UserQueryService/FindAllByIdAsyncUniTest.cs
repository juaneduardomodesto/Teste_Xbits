using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.UserResponse;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Service.UserService.UserQueryService.Base;

namespace Teste_Xbits.Service.UserService.UserQueryService;

public class FindAllByIdAsyncUniTest : UserQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Perfect setting")]
    public async Task FindAllWithPaginationAsync_ValidUser_ReturnTrue()
    {
        var pageParams = CreatePageParams();
        var user = CreateValidUser();
        var users = new List<User> { user };
    
        var domainPageList = new PageList<User>(users, 1, 1, 10);
    
        var userResponse = users.Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            AcceptPrivacyPolicy = u.AcceptPrivacyPolicy,
            AcceptTermsOfUse = u.AcceptTermsOfUse,
            Cpf = u.Cpf,
            IsActive = u.IsActive,
            Role = u.Role,
        }).ToList();

        var responsePageList = new PageList<UserResponse>(userResponse, 1, 1, 10);

        UserRepository
            .Setup(x => x.FindAllWithPaginationAsync(
                pageParams,
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .ReturnsAsync(domainPageList);

        UserMapper
            .Setup(x => x.DomainToPaginationUserResponse(domainPageList))
            .Returns(responsePageList);
        
        var result = await UserQueryService.FindAllWithPaginationAsync(null, null, null, pageParams);


        Assert.NotEmpty(result.Items);
        Assert.Equal(1, result.TotalCount);
    
        UserRepository
            .Verify(x => x.FindAllWithPaginationAsync(
                pageParams,
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()), Times.Once);
        
        UserMapper
            .Verify(x => x.DomainToPaginationUserResponse(domainPageList), Times.Once);
    }

    [Fact]
    [Trait("Query", "Empty list")]
    public async Task FindAllWithPaginationAsync_EmptyList_ReturnEmptyList()
    {
        var pageParams = CreatePageParams();
        var emptyUsersList = new List<User>();
    
        var domainPageList = new PageList<User>(emptyUsersList, 0, 1, 10);

        UserRepository
            .Setup(x => x.FindAllWithPaginationAsync(
                It.IsAny<PageParams>(),
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .ReturnsAsync(domainPageList);
        
        var result = await UserQueryService.FindAllWithPaginationAsync(null, null, null, pageParams);
        
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    
        UserRepository
            .Verify(x => x.FindAllWithPaginationAsync(
                It.IsAny<PageParams>(),
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()), Times.Once);
        
        UserMapper
            .Verify(x => x.DomainToPaginationUserResponse(It.IsAny<PageList<User>>()), Times.Never);
    }
}