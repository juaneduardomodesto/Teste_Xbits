using FluentValidation;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;
using Teste_Xbits.Infra.Repositories;

namespace Teste_Xbits.ApplicationService.Services.Register;

public class UserCommandService : ServiceBase<User>, IUserCommandService
{
    private readonly INotificationHandler _notificationHandler;
    private readonly IValidate<User> _validate;
    private readonly ILoggerHandler _loggerHandler;
    private readonly IUserMapper _userMapper;
    private readonly IUserRepository _userRepository;
    
    public UserCommandService(
        INotificationHandler notification,
        IValidate<User> validate,
        ILoggerHandler logger,
        IUserMapper userMapper,
        IUserRepository userRepository)
        : base(notification, validate, logger)
    {
        this._notificationHandler = notification;
        this._validate = validate;
        this._loggerHandler = logger;
        this._userMapper = userMapper;
        this._userRepository = userRepository;
    }

    public async Task<bool> RegisterUser(UserRegisterRequest dtoRegister)
    {
        #region Validations
        
        var preExist = await _userRepository.FindByPredicateAsync(
            x => x.Email == dtoRegister.Email);
        if (preExist != null)
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Exist.GetDescription().FormatTo("Usuário"));

        if (string.IsNullOrEmpty(dtoRegister.Name))
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("Nome de usuário"));
            return false;
        }

        if (string.IsNullOrEmpty(dtoRegister.Email))
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("E-mail"));
            return false;
        }
        
        if(!EmailExtension.ValidateEmail(dtoRegister.Email))
            return false;

        if (string.IsNullOrEmpty(dtoRegister.Password))
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("Senha"));
            return false;
        }

        if (string.IsNullOrEmpty(dtoRegister.ConfirmPassword))
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("Confirmação de senha"));
            return false;
        }

        if (dtoRegister.Password != dtoRegister.ConfirmPassword)
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.PasswordNotMatch.GetDescription());
        }

        #endregion
        
        var mappedUser = _userMapper.DtoRegisterToDomain(dtoRegister);
        if (!await EntityValidationAsync(mappedUser)) return false;
        
        var user = await _userRepository.SaveAsync(mappedUser);
        return true;
    }
}