using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Service.TokenService.Base;

namespace Teste_Xbits.Service.TokenService;

public class AuthenticationAsyncUnitTest : TokenCommandServiceSetup
{
    [Fact]
    [Trait("Command", "Perfect setting")]
    public async Task Authentication_ValidRequest_ReturnsTokenResponse()
    {
        var loginRequest = CreateValidLoginRequest();
        var userGuid = Guid.NewGuid();
        var role = ERoles.Administrator;
        var expectedTokenResponse = CreateTokenResponse();

        TokenMapper
            .Setup(x => x.MapToTokenResponse(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .Returns(expectedTokenResponse);

        // Act
        var result = await TokenCommandService.AuthenticationAsync(loginRequest, userGuid, role);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTokenResponse.AccessToken, result.AccessToken);
        
        TokenMapper.Verify(x => x.MapToTokenResponse(
            It.IsAny<string>(),
            loginRequest.Email!,
            It.IsAny<int>()), Times.Once);
    }

    [Fact]
    [Trait("Command", "Different roles")]
    public async Task Authentication_DifferentRoles_ReturnsTokenResponse()
    {
        var loginRequest = CreateValidLoginRequest();
        var userGuid = Guid.NewGuid();
        var roles = new[] { ERoles.Administrator, ERoles.Employee, ERoles.Administrator };
        var expectedTokenResponse = CreateTokenResponse();

        TokenMapper
            .Setup(x => x.MapToTokenResponse(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .Returns(expectedTokenResponse);

        foreach (var role in roles)
        {
            var result = await TokenCommandService.AuthenticationAsync(loginRequest, userGuid, role);
            
            Assert.NotNull(result);
            Assert.Equal(expectedTokenResponse.AccessToken, result.AccessToken);
        }

        TokenMapper.Verify(x => x.MapToTokenResponse(
            It.IsAny<string>(),
            loginRequest.Email!,
            It.IsAny<int>()), Times.Exactly(roles.Length));
    }

    [Fact]
    [Trait("Command", "Configuration values")]
    public async Task Authentication_UsesCorrectConfigurationValues()
    {
        var loginRequest = CreateValidLoginRequest();
        var userGuid = Guid.NewGuid();
        var role = ERoles.Administrator;
        
        Configuration.Setup(x => x["Jwt:Issuer"]).Returns("custom-issuer");
        Configuration.Setup(x => x["Jwt:Audience"]).Returns("custom-audience");
        Configuration.Setup(x => x["Jwt:DurationInMinutes"]).Returns("60");

        TokenMapper
            .Setup(x => x.MapToTokenResponse(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .Returns(CreateTokenResponse());
        
        var result = await TokenCommandService.AuthenticationAsync(loginRequest, userGuid, role);
        
        Assert.NotNull(result);
        Configuration.Verify(x => x["Jwt:Issuer"], Times.AtLeastOnce);
        Configuration.Verify(x => x["Jwt:Audience"], Times.AtLeastOnce);
        Configuration.Verify(x => x["Jwt:DurationInMinutes"], Times.AtLeastOnce);
    }

    [Fact]
    [Trait("Command", "Claims validation")]
    public async Task Authentication_IncludesCorrectClaims()
    {
        var loginRequest = CreateValidLoginRequest();
        var userGuid = Guid.NewGuid();
        var role = ERoles.Administrator;

        TokenMapper
            .Setup(x => x.MapToTokenResponse(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .Returns(CreateTokenResponse());
        
        var result = await TokenCommandService.AuthenticationAsync(loginRequest, userGuid, role);

        Assert.NotNull(result);
        TokenMapper.Verify(x => x.MapToTokenResponse(
            It.Is<string>(token => !string.IsNullOrEmpty(token)),
            loginRequest.Email!,
            It.IsAny<int>()), Times.Once);
    }

    [Fact]
    [Trait("Command", "Null mapper response")]
    public async Task Authentication_NullMapperResponse_ReturnsNull()
    {
        var loginRequest = CreateValidLoginRequest();
        var userGuid = Guid.NewGuid();
        var role = ERoles.Administrator;

        TokenMapper
            .Setup(x => x.MapToTokenResponse(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .Returns((TokenResponse)null!);
        
        var result = await TokenCommandService.AuthenticationAsync(loginRequest, userGuid, role);
        
        Assert.Null(result);
        TokenMapper.Verify(x => x.MapToTokenResponse(
            It.IsAny<string>(),
            loginRequest.Email!,
            It.IsAny<int>()), Times.Once);
    }
}