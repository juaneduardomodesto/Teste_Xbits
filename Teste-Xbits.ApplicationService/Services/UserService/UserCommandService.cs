using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.UserService;

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

    public async Task<bool> RegisterUserAsync(UserRegisterRequest dtoRegister)
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

    public async Task<bool> UpdateUserAsync(UserUpdateRequest dtoUpdate)
    {
        #region Validations
        
        if (dtoUpdate.Id == null)
        {
            _notificationHandler.CreateNotification(
                "Delete",
                EMessage.InvalidId.GetDescription().FormatTo("Id"));
        }
        
        if (string.IsNullOrEmpty(dtoUpdate.Name))
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("Nome de usuário"));
            return false;
        }

        if (string.IsNullOrEmpty(dtoUpdate.Email))
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("E-mail"));
            return false;
        }
        
        if(!EmailExtension.ValidateEmail(dtoUpdate.Email))
            return false;

        if (string.IsNullOrEmpty(dtoUpdate.Password))
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("Senha"));
            return false;
        }

        if (string.IsNullOrEmpty(dtoUpdate.ConfirmPassword))
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("Confirmação de senha"));
            return false;
        }

        if (dtoUpdate.Password != dtoUpdate.ConfirmPassword)
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.PasswordNotMatch.GetDescription());
        }
        
        #endregion
        
        var user = await _userRepository.FindByPredicateAsync(x => x.Id == dtoUpdate.Id, asNoTracking: true);
        if (user == null)
        {
            _notificationHandler.CreateNotification(
                "Delete",
                EMessage.UserNotFound.GetDescription());
        }

        var updatedUser = _userMapper.DtoUpdateToDomain(dtoUpdate, user!.Id);
        if(!await EntityValidationAsync(updatedUser)) return false;
        
        return await _userRepository.UpdateAsync(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(UserDeleteRequest dtoDelete)
    {
        if (dtoDelete.Id == null)
        {
            _notificationHandler.CreateNotification(
                "Delete",
                EMessage.InvalidId.GetDescription().FormatTo("Id"));
        }
        
        var user = await _userRepository.FindByPredicateAsync(x => x.Id == dtoDelete.Id);
        if (user == null)
        {
            _notificationHandler.CreateNotification(
                "Delete",
                EMessage.UserNotFound.GetDescription());
        }
        
        if(await EntityValidationAsync(user!))  return false;
        
        return await _userRepository.DeleteAsync(user!);
    }
}