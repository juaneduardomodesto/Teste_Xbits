using System.Linq.Expressions;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Service.LoginService.Base;

namespace Teste_Xbits.Service.LoginService;

public class LoginQueryServiceUnitTest : LoginQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Perfect setting.")]
    public async Task LoginAsync_ValidLogin_ReturnUser()
    {
        var dtoLogin = CreateValidLoginRequest();
        var user = CreateValidUser();
        var loginResponse = CreateValidLoginResponse();
        var tokenResponse = CreateValidTokenResponse();
        
        var expectedSalt = "test-salt";
        var passwordHash = dtoLogin.Password!.ConvertMd5(expectedSalt);
    
        Assert.NotNull(user);
        Assert.NotNull(user.PasswordHash);
        Assert.Equal(user.PasswordHash, passwordHash);
    
        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);
    
        TokenCommandService
            .Setup(x => x.Authentication(
                It.IsAny<LoginRequest>(), 
                It.IsAny<Guid>(), 
                It.IsAny<ERoles>()))
            .ReturnsAsync(tokenResponse);
    
        LoginMapper
            .Setup(x => x.DtoToLoginResponse(It.IsAny<LoginRequest>(), It.IsAny<TokenResponse>()))
            .Returns(loginResponse);
    
        var result = await LoginQueryService.LoginAsync(dtoLogin);
    
        Assert.NotNull(result);
        Assert.Equal(loginResponse.UserIdentifier, result.UserIdentifier);
        Assert.Equal(loginResponse.Token, result.Token);
        Assert.Equal(loginResponse.ExpireIn, result.ExpireIn);
    
        UserRepository.Verify(x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IQueryable<User>>?>(),
            It.IsAny<bool>()), Times.Once);
    
        TokenCommandService.Verify(x => x.Authentication(
                dtoLogin, 
                It.IsAny<Guid>(), 
                user.Role),
            Times.Once);
        
        LoginMapper.Verify(x => x.DtoToLoginResponse(dtoLogin, tokenResponse), Times.Once);
    }
    
    [Fact]
    [Trait("Query", "Invalid credentials.")]
    public async Task LoginAsync_InvalidPassword_ReturnNullAndCreateNotification()
    {
        var dtoLogin = CreateInvalidLoginRequest();
        var user = CreateValidUser();

        CreateNotification();
        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);
        
        var result = await LoginQueryService.LoginAsync(dtoLogin);
        
        Assert.Null(result);
    
        UserRepository.Verify(x => x.FindByPredicateAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IQueryable<User>>?>(),
            It.IsAny<bool>()), Times.Once);
        
        NotificationHandler.Verify(x => x.CreateNotification(
            LoginTrace.Login,
            EMessage.InvalidCredentials.GetDescription()), Times.Once);
    
        TokenCommandService.Verify(x => x.Authentication(
            It.IsAny<LoginRequest>(),
            It.IsAny<Guid>(),
            It.IsAny<ERoles>()), Times.Never);
    
        LoginMapper.Verify(x => x.DtoToLoginResponse(
            It.IsAny<LoginRequest>(),
            It.IsAny<TokenResponse>()), Times.Never);
    }
    
    [Fact]
    [Trait("Query", "User not found.")]
    public async Task LoginAsync_UserNotFound_ReturnNullAndNotification()
    {
        var dtoLogin = CreateValidLoginRequest();

        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync((User)null!);
        
        var result = await LoginQueryService.LoginAsync(dtoLogin);
        
        Assert.Null(result);
        NotificationHandler.Verify(x => x.CreateNotification(
            LoginTrace.Login,
            EMessage.UserNotFound.GetDescription()), Times.Once);
    }
    
    [Fact]
    [Trait("Query", "Inactive user.")]
    public async Task LoginAsync_InactiveUser_ReturnNullAndNotification()
    {
        var dtoLogin = CreateValidLoginRequest();
        var user = CreateValidUser();
        user.IsActive = false;

        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);
        
        var result = await LoginQueryService.LoginAsync(dtoLogin);
        
        Assert.Null(result);
        NotificationHandler.Verify(x => x.CreateNotification(
            LoginTrace.Login,
            EMessage.InactiveUser.GetDescription()), Times.Once);
    }
    
    [Fact]
    [Trait("Query", "Invalid password.")]
    public async Task LoginAsync_InvalidPassword_ReturnNullAndNotification()
    {
        var dtoLogin = CreateInvalidLoginRequest();
        var user = CreateValidUser();
    
        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);
        
        var result = await LoginQueryService.LoginAsync(dtoLogin);
        
        Assert.Null(result);
        NotificationHandler.Verify(x => x.CreateNotification(
            LoginTrace.Login,
            EMessage.InvalidCredentials.GetDescription()), Times.Once);
    }

    [Fact]
    [Trait("Query", "Token generation failed.")]
    public async Task LoginAsync_TokenGenerationFailed_ReturnNullAndNotification()
    {
        var dtoLogin = CreateValidLoginRequest();
        var user = CreateValidUser();

        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);

        TokenCommandService
            .Setup(x => x.Authentication(
                It.IsAny<LoginRequest>(), 
                It.IsAny<Guid>(), 
                It.IsAny<ERoles>()))
            .ReturnsAsync((TokenResponse)null!);
        
        var result = await LoginQueryService.LoginAsync(dtoLogin);
        
        Assert.Null(result);
        NotificationHandler.Verify(x => x.CreateNotification(
            LoginTrace.Login,
            EMessage.TokenError.GetDescription()), Times.Once);
    }
}