using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.UserResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.Service.UserService.UserQueryService.Base;

public class UserQueryServiceSetup
{
    protected readonly Mock<IUserRepository> UserRepository;
    protected readonly Mock<IUserMapper> UserMapper;
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<IValidate<User>> Validator;
    protected readonly Mock<ILoggerHandler> LoggerHandler;
    protected readonly ApplicationService.Services.UserService.UserQueryService UserQueryService;

    protected UserQueryServiceSetup()
    {
        UserRepository = new Mock<IUserRepository>();
        UserMapper = new Mock<IUserMapper>();
        NotificationHandler = new Mock<INotificationHandler>();
        Validator = new Mock<IValidate<User>>();
        LoggerHandler = new Mock<ILoggerHandler>();

        UserQueryService = new ApplicationService.Services.UserService.UserQueryService(
            NotificationHandler.Object,
            Validator.Object,
            LoggerHandler.Object,
            UserRepository.Object,
            UserMapper.Object
        );
    }

    protected PageParams CreatePageParams(int pageNumber = 1, int pageSize = 10)
    {
        return new PageParams { PageNumber = pageNumber, PageSize = pageSize };
    }

    protected PageList<User> CreateUserPageList(int totalItems = 1)
    {
        var users = new List<User>();
        if (totalItems > 0)
        {
            users.Add(CreateValidUser());
        }

        return new PageList<User>(users, totalItems, 1, 10);
    }

    protected User CreateValidUser()
    {
        return new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            Cpf = "13193568937",
            IsActive = true,
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true,
            PasswordHash = "password123",
            Role = ERoles.Administrator,
            CreatedAt = DateTime.Today,
            UpdatedAt = DateTime.Today,
        };
    }

    protected UserResponse CreateUserResponse()
    {
        return new UserResponse
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            Cpf = "13193568937",
            IsActive = true,
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true,
            Role = ERoles.Administrator,
        };
    }
}