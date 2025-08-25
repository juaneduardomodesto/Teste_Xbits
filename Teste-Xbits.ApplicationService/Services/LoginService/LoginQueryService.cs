using Microsoft.Extensions.Configuration;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.LoginResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.LoginService;

public class LoginQueryService : ServiceBase<Login>, ILoginQueryService
{
    private readonly INotificationHandler _notificationHandler;
    private readonly IValidate<Login> _validate;
    private readonly ILoggerHandler _loggerHandler;
    private readonly IUserMapper _userMapper;
    private readonly IUserRepository _userRepository;
    private readonly string _applicationSalt;
    private readonly ITokenCommandService  _tokenCommandService;
    private readonly ILoginMapper _loginMapper;

    public LoginQueryService(
        INotificationHandler notification,
        IValidate<Login> validate,
        ILoggerHandler logger,
        IUserMapper userMapper,
        IUserRepository userRepository,
        IConfiguration configuration,
        ITokenCommandService tokenCommandService,
        ILoginMapper loginMapper)
        : base(notification, validate, logger)
    {
        this._notificationHandler = notification;
        this._validate = validate;
        this._loggerHandler = logger;
        this._userMapper = userMapper;
        this._userRepository = userRepository;
        this._applicationSalt = configuration["Security:PasswordSalt"];
        this._tokenCommandService = tokenCommandService;
        this._loginMapper = loginMapper;
    }

    public async Task<LoginResponse?> Login(LoginRequest dtoLogin)
    {
        #region validation

        if (string.IsNullOrEmpty(dtoLogin.Email))
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.Required.GetDescription().FormatTo("E-mail"));
            return null;
        }

        if (string.IsNullOrEmpty(dtoLogin.Password))
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.Required.GetDescription().FormatTo("Senha"));
            return null;
        }

        var user = await _userRepository.FindByPredicateAsync(x =>
            x.Email == dtoLogin.Email || x.Cpf == dtoLogin.Email);
        if (user == null)
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.UserNotFound.GetDescription());
            return null;
        }

        if (!user.IsActive)
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.InactiveUser.GetDescription());
            return null;
        }

        if (!user.PasswordHash.Equals(CryptographyExtension.ConvertMd5(dtoLogin.Password, _applicationSalt)))
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.InvalidCredentials.GetDescription());
            return null;
        }

        #endregion
        
        var token = await _tokenCommandService.Authentication(dtoLogin);
        if (token == null)
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.TokenError.GetDescription());
            return null;
        }
        
        var loginResponse = _loginMapper.DtoToLoginResponse(dtoLogin, token);
        return loginResponse;
    }
}