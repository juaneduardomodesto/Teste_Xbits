using Microsoft.Extensions.Configuration;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.LoginResponse;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.ApplicationService.Services.LoginService;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.Service.LoginService.Base;

public class LoginQueryServiceSetup
{
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<IValidate<Login>> Validator;
    protected readonly Mock<ILoggerHandler> LoggerHandler;
    protected readonly Mock<IUserRepository> UserRepository;
    protected readonly Mock<IConfiguration> Configuration;
    protected readonly Mock<ITokenCommandService> TokenCommandService;
    protected readonly Mock<ILoginMapper> LoginMapper;
    protected readonly Dictionary<string, string> Errors;
    protected readonly ValidationResponse ValidationResponse;
    protected readonly LoginQueryService LoginQueryService;

    protected LoginQueryServiceSetup()
    {
        NotificationHandler = new Mock<INotificationHandler>();
        Validator = new Mock<IValidate<Login>>();
        LoggerHandler = new Mock<ILoggerHandler>();
        UserRepository = new Mock<IUserRepository>();
        Configuration = new Mock<IConfiguration>();
        TokenCommandService = new Mock<ITokenCommandService>();
        LoginMapper = new Mock<ILoginMapper>();

        Errors = [];
        ValidationResponse = ValidationResponse.CreateResponse(Errors);
        
        Configuration.Setup(x => x["Security:PasswordSalt"]).Returns("test-salt");

        LoginQueryService = new LoginQueryService(
            NotificationHandler.Object,
            Validator.Object,
            LoggerHandler.Object,
            UserRepository.Object,
            Configuration.Object,
            TokenCommandService.Object,
            LoginMapper.Object
        );
    }
    
    protected void CreateNotification() => Errors.Add("Error", "Error");
    
    protected LoginRequest CreateValidLoginRequest()
    {
        return new LoginRequest
        {
            Email = "test@email.com",
            Password = "password123"
        };
    }
    
    protected LoginRequest CreateInvalidLoginRequest()
    {
        return new LoginRequest
        {
            Email = "test@email.com",
            Password = "badpassword123"
        };
    }

    protected User CreateValidUser()
    {
        return new User
        {
            Id = 1L,
            Email = "test@email.com",
            Cpf = "12345678900",
            PasswordHash = "password123".ConvertMd5("test-salt"),
            IsActive = true,
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true,
            Name = "user unitest",
            CreatedAt = DateTime.Today,
            UpdatedAt = DateTime.Today,
            Role = ERoles.Administrator
        };
    }

    protected LoginResponse CreateValidLoginResponse()
    {
        return new LoginResponse
        {
            UserIdentifier = "test@email.com",
            Token = "valid-token",
            ExpireIn = 1000000
        };
    }

    protected TokenResponse CreateValidTokenResponse()
    {
        return new TokenResponse
        {
            AccessToken = "valid-token",
            ExpireIn = 1000000,
            UserIdentifier = "identificador@gmail.com"
        };
    }
}