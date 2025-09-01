using Microsoft.Extensions.Configuration;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.LoginResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.LoginService;

public class LoginQueryService(
    INotificationHandler notification,
    IValidate<Login> validate,
    ILoggerHandler logger,
    IUserRepository userRepository,
    IConfiguration configuration,
    ITokenCommandService tokenCommandService,
    ILoginMapper loginMapper)
    : ServiceBase<Login>(notification, validate, logger), ILoginQueryService
{
    private readonly INotificationHandler _notificationHandler = notification;
    private readonly ILoggerHandler _loggerHandler = logger;
    private readonly string? _applicationSalt = configuration["Security:PasswordSalt"];

    public async Task<LoginResponse?> LoginAsync(LoginRequest dtoLogin)
    {
        #region validation

        if (string.IsNullOrEmpty(dtoLogin.Email))
        {
            _notificationHandler.CreateNotification(
                LoginTrace.Login,
                EMessage.Required.GetDescription().FormatTo("E-mail"));
            return null;
        }

        if (string.IsNullOrEmpty(dtoLogin.Password))
        {
            _notificationHandler.CreateNotification(
                LoginTrace.Login,
                EMessage.Required.GetDescription().FormatTo("Senha"));
            return null;
        }

        var user = await userRepository.FindByPredicateAsync(x =>
            x.Email == dtoLogin.Email || x.Cpf == dtoLogin.Email);
        if (user == null)
        {
            _notificationHandler.CreateNotification(
                LoginTrace.Login,
                EMessage.UserNotFound.GetDescription());
            return null;
        }

        if (!user.IsActive)
        {
            _notificationHandler.CreateNotification(
                LoginTrace.Login,
                EMessage.InactiveUser.GetDescription());
            return null;
        }

        if (!user.PasswordHash.Equals(dtoLogin.Password.ConvertMd5(_applicationSalt!)))
        {
            _notificationHandler.CreateNotification(
                LoginTrace.Login,
                EMessage.InvalidCredentials.GetDescription());
            return null;
        }

        #endregion
        
        var token = await tokenCommandService.Authentication(dtoLogin, Guid.NewGuid());
        if (token == null)
        {
            _notificationHandler.CreateNotification(
                LoginTrace.Login,
                EMessage.TokenError.GetDescription());
            return null;
        }
        
        var loginResponse = loginMapper.DtoToLoginResponse(dtoLogin, token);
        return loginResponse;
    }
}