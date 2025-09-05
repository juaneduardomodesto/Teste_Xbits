using Microsoft.Extensions.Configuration;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Interface;

namespace Teste_Xbits.Service.TokenService.Base;

public class TokenCommandServiceSetup
{
    protected readonly Mock<IConfiguration> Configuration;
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<ITokenMapper> TokenMapper;
    protected readonly Mock<IValidate<Domain.Entities.Token>> Validator;
    protected readonly Mock<ILoggerHandler> LoggerHandler;
    protected readonly ApplicationService.Services.TokenService.TokenCommandCommandService TokenCommandService;
    protected readonly Dictionary<string, string> Errors;
    protected readonly ValidationResponse ValidationResponse;

    protected TokenCommandServiceSetup()
    {
        Configuration = new Mock<IConfiguration>();
        NotificationHandler = new Mock<INotificationHandler>();
        TokenMapper = new Mock<ITokenMapper>();
        Validator = new Mock<IValidate<Domain.Entities.Token>>();
        LoggerHandler = new Mock<ILoggerHandler>();
        
        Errors = [];
        ValidationResponse = ValidationResponse.CreateResponse(Errors);
        
        Configuration.Setup(x => x["Jwt:Issuer"]).Returns("test-issuer");
        Configuration.Setup(x => x["Jwt:Audience"]).Returns("test-audience");
        Configuration.Setup(x => x["Jwt:JwtKey"]).Returns("test-jwt-key-very-long-secret-key-for-testing");
        Configuration.Setup(x => x["Jwt:DurationInMinutes"]).Returns("30");

        TokenCommandService = new ApplicationService.Services.TokenService.TokenCommandCommandService(
            Configuration.Object,
            NotificationHandler.Object,
            TokenMapper.Object,
            Validator.Object,
            LoggerHandler.Object
        );
    }

    protected LoginRequest CreateValidLoginRequest() => new()
    {
        Email = "test@example.com",
        Password = "Password123!"
    };

    protected TokenResponse CreateTokenResponse() => new()
    {
        AccessToken = "test-access-token",
        UserIdentifier = "test@example.com",
        ExpireIn = 1800
    };

    protected void SetupTokenMapperMapToTokenResponse(TokenResponse tokenResponse)
    {
        TokenMapper
            .Setup(x => x.MapToTokenResponse(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .Returns(tokenResponse);
    }
}