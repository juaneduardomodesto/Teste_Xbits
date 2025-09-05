using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.Service.UserService.UserCommandService.Base;

public class UserCommandServiceSetup
{
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<IValidate<User>> Validator;
    protected readonly Mock<ILoggerHandler> LoggerHandler;
    protected readonly Mock<IUserRepository> UserRepository;
    protected readonly Mock<IConfiguration> Configuration;
    protected readonly Mock<IUserMapper> UserMapper;
    protected readonly Dictionary<string, string> Errors;
    protected readonly ValidationResponse ValidationResponse;
    protected readonly ApplicationService.Services.UserService.UserCommandService UserCommandService;

    protected UserCommandServiceSetup()
    {
        NotificationHandler = new Mock<INotificationHandler>();
        Validator = new Mock<IValidate<User>>();
        LoggerHandler = new Mock<ILoggerHandler>();
        UserRepository = new Mock<IUserRepository>();
        UserMapper = new Mock<IUserMapper>();
        Configuration = new Mock<IConfiguration>();

        Errors = [];
        ValidationResponse = ValidationResponse.CreateResponse(Errors);

        UserCommandService = new ApplicationService.Services.UserService.UserCommandService(
            NotificationHandler.Object,
            Validator.Object,
            LoggerHandler.Object,
            UserMapper.Object,
            UserRepository.Object
        );
    }

    protected void CreateNotification() => Errors.Add("Error", "Error");

    protected User CreateValidUser()
    {
        return new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "Password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true,
            Cpf = "13193568937",
            IsActive = true,
            Role = ERoles.Administrator,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    protected User CreateInvalidUser()
    {
        return new User
        {
            Id = 0,
            Name = "",
            Email = "invalid-email",
            PasswordHash = "",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true,
            Cpf = "13193568937",
            IsActive = true,
            Role = ERoles.Administrator,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    protected UserRegisterRequest CreateValidUserRegisterRequest() =>
        new()
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "Password123",
            ConfirmPassword = "Password123",
            Roles = ERoles.Administrator,
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true,
            Cpf = "13193568937",
            IsActive = true
        };

    protected UserRegisterRequest CreateInvalidUserRegisterRequest() =>
        new()
        {
            Name = "invalide user",
            Email = "invalidemail@gmail.com",
            Password = "Password123",
            ConfirmPassword = "DifferentPassword",
            Roles = ERoles.Administrator,
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true,
            Cpf = "13193568937",
            IsActive = true
        };

    protected UserUpdateRequest CreateValidUserUpdateRequest()
    {
        return new UserUpdateRequest
        {
            Id = 1L,
            Name = "Updated User",
            Email = "updated@example.com",
            Password = "NewPassword123",
            ConfirmPassword = "NewPassword123",
            Roles = ERoles.Administrator,
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true,
            Cpf = "13193568937",
            IsActive = true
        };
    }

    protected UserUpdateRequest CreateInvalidUserUpdateRequest()
    {
        return new UserUpdateRequest
        {
            Id = 0L,
            Name = "",
            Email = "invalid-email",
            Password = "Password123",
            ConfirmPassword = "DifferentPassword",
            Roles = ERoles.Administrator,
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true,
            Cpf = "13193568937",
            IsActive = true
        };
    }

    protected UserDeleteRequest CreateUserDeleteRequest()
    {
        return new UserDeleteRequest()
        {
            Id = 1L
        };
    }

    protected UserDeleteRequest CreateInvalidUserDeleteRequest()
    {
        return new UserDeleteRequest()
        {
            Id = 0L
        };
    }

    protected UserCredential CreateUserCredential()
    {
        return new UserCredential
        {
            Id = Guid.NewGuid(),
            Roles = []
        };
    }

    protected void SetupUserRepositoryFindByPredicateAsync(User user)
    {
        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                null, true))
            .ReturnsAsync(user);
    }
    
    protected void SetupUserRepositoryFindByPredicateQueryableAsync(User user)
    {
        UserRepository
            .Setup(x => x.FindByPredicateAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(user);
    }
    protected void SetupUserMapperDtoUpdateToDomain(UserUpdateRequest userUpdateRequest, long userId, User user)
    {
        UserMapper.Setup(x => x.DtoUpdateToDomain(userUpdateRequest, userId)).Returns(user);
    }

    protected void SetupValidatorValidationAsync()
    {
        Validator.Setup(x => x.ValidationAsync(It.IsAny<User>())).ReturnsAsync(ValidationResponse);
    }

    protected void SetupUserRepositoryUpdateAsync(User user, bool returnValue)
    {
        UserRepository.Setup(x => x.UpdateAsync(user)).ReturnsAsync(returnValue);
    }

    protected void SetupLoggerHandlerCreateLogger()
    {
        LoggerHandler.Setup(x => x.CreateLogger(It.IsAny<DomainLogger>()));
    }

    protected void SetupUserMapperDtoRegisterToDomain(UserRegisterRequest dtoRegister, User user)
    {
        UserMapper.Setup(x => x.DtoRegisterToDomain(dtoRegister))
            .Returns(user);
    }

    protected void SetupUserRepositorySaveAsync(User user, bool returnValue)
    {
        UserRepository.Setup(x => x.SaveAsync(user))
            .ReturnsAsync(returnValue);    
    }

    protected void SetupUserRepositoryDeleteAsync(User user, bool returnValue)
    {
        UserRepository.Setup(x => x.DeleteAsync(user))
            .ReturnsAsync(returnValue);
    }
}