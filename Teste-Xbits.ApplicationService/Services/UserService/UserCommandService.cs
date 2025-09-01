using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.UserService;

public class UserCommandService(
    INotificationHandler notification,
    IValidate<User> validate,
    ILoggerHandler logger,
    IUserMapper userMapper,
    IUserRepository userRepository)
    : ServiceBase<User>(notification, validate, logger), IUserCommandService
{
    private readonly INotificationHandler _notificationHandler = notification;
    private readonly ILoggerHandler _loggerHandler = logger;

    public async Task<bool> RegisterUserAsync(UserRegisterRequest dtoRegister)
    {
        #region Validations
        
        var preExist = await userRepository.FindByPredicateAsync(
            x => x.Email == dtoRegister.Email);
        if (preExist != null)
            _notificationHandler.CreateNotification(
                UserTracer.Save,
                EMessage.Exist.GetDescription().FormatTo("Usuário"));

        if (string.IsNullOrEmpty(dtoRegister.Name))
        {
            _notificationHandler.CreateNotification(
                UserTracer.Save,
                EMessage.Required.GetDescription().FormatTo("Nome de usuário"));
            return false;
        }

        if (string.IsNullOrEmpty(dtoRegister.Email))
        {
            _notificationHandler.CreateNotification(
                UserTracer.Save,
                EMessage.Required.GetDescription().FormatTo("E-mail"));
            return false;
        }
        
        if(!dtoRegister.Email.ValidateEmail())
            return false;

        if (string.IsNullOrEmpty(dtoRegister.Password))
        {
            _notificationHandler.CreateNotification(
                UserTracer.Save,
                EMessage.Required.GetDescription().FormatTo("Senha"));
            return false;
        }

        if (string.IsNullOrEmpty(dtoRegister.ConfirmPassword))
        {
            _notificationHandler.CreateNotification(
                UserTracer.Save,
                EMessage.Required.GetDescription().FormatTo("Confirmação de senha"));
            return false;
        }

        if (dtoRegister.Password != dtoRegister.ConfirmPassword)
        {
            _notificationHandler.CreateNotification(
                UserTracer.Save,
                EMessage.PasswordNotMatch.GetDescription());
        }

        #endregion
        
        var mappedUser = userMapper.DtoRegisterToDomain(dtoRegister);
        if (!await EntityValidationAsync(mappedUser)) return false;
        
        _ = await userRepository.SaveAsync(mappedUser);
        return true;
    }

    public async Task<bool> UpdateUserAsync(UserUpdateRequest dtoUpdate)
    {
        #region Validations
        
        if (dtoUpdate.Id == 0)
        {
            _notificationHandler.CreateNotification(
                UserTracer.Update,
                EMessage.InvalidId.GetDescription().FormatTo("Id"));
        }
        
        if (string.IsNullOrEmpty(dtoUpdate.Name))
        {
            _notificationHandler.CreateNotification(
                UserTracer.Update,
                EMessage.Required.GetDescription().FormatTo("Nome de usuário"));
            return false;
        }

        if (string.IsNullOrEmpty(dtoUpdate.Email))
        {
            _notificationHandler.CreateNotification(
                UserTracer.Update,
                EMessage.Required.GetDescription().FormatTo("E-mail"));
            return false;
        }
        
        if(!dtoUpdate.Email.ValidateEmail())
            return false;

        if (string.IsNullOrEmpty(dtoUpdate.Password))
        {
            _notificationHandler.CreateNotification(
                UserTracer.Update,
                EMessage.Required.GetDescription().FormatTo("Senha"));
            return false;
        }

        if (string.IsNullOrEmpty(dtoUpdate.ConfirmPassword))
        {
            _notificationHandler.CreateNotification(
                UserTracer.Update,
                EMessage.Required.GetDescription().FormatTo("Confirmação de senha"));
            return false;
        }

        if (dtoUpdate.Password != dtoUpdate.ConfirmPassword)
        {
            _notificationHandler.CreateNotification(
                UserTracer.Update,
                EMessage.PasswordNotMatch.GetDescription());
            return false;
        }
        
        #endregion
        
        var user = await userRepository.FindByPredicateAsync(x => x.Id == dtoUpdate.Id, asNoTracking: true);
        if (user == null)
        {
            _notificationHandler.CreateNotification(
                UserTracer.Update,
                EMessage.UserNotFound.GetDescription());
            return false;
        }

        var updatedUser = userMapper.DtoUpdateToDomain(dtoUpdate, user.Id);
        if(!await EntityValidationAsync(updatedUser)) return false;
        
        return await userRepository.UpdateAsync(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(UserDeleteRequest dtoDelete)
    {
        if (dtoDelete.Id == 0)
        {
            _notificationHandler.CreateNotification(
                UserTracer.Delete,
                EMessage.InvalidId.GetDescription().FormatTo("Id"));
        }
        
        var user = await userRepository.FindByPredicateAsync(x => x.Id == dtoDelete.Id);
        if (user == null)
        {
            _notificationHandler.CreateNotification(
                UserTracer.Delete,
                EMessage.UserNotFound.GetDescription());
        }
        
        if(await EntityValidationAsync(user!))  return false;
        
        return await userRepository.DeleteAsync(user!);
    }
}