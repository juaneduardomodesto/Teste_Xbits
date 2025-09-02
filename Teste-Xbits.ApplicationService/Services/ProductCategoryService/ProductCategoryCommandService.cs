using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.ProductCategoryService;

public class ProductCategoryCommandService(
    INotificationHandler notification,
    IValidate<ProductCategory> validate,
    ILoggerHandler logger,
    IProductCategoryMapper productCategoryMapper,
    IProductCategoryRepository productCategoryRepository)
    : ServiceBase<ProductCategory>(notification, validate, logger), IProductCategoryCommandService
{
    private readonly INotificationHandler _notificationHandler = notification;
    private readonly ILoggerHandler _loggerHandler = logger;

    public async Task<bool> RegisterAsync(ProductCategoryRegisterRequest dtoRegister, UserCredential userCredential)
    {
        #region Validation

        if (dtoRegister.Name == null)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Save,
                EMessage.Required.GetDescription().FormatTo("Nome"));
            return false;
        }
        
        var preExist = await productCategoryRepository.FindByPredicateAsync(
            x =>x.ProductCategoryCode == dtoRegister.Code);
        if (preExist != null)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Save,
                EMessage.Exist.GetDescription().FormatTo("Usuário"));
            return false;
        }

        if (dtoRegister.Description == null)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Save,
                EMessage.Required.GetDescription().FormatTo("Descrição"));
            return false;
        }

        if (dtoRegister.Code == null)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Save,
                EMessage.Required.GetDescription().FormatTo("Code"));
            return false;
        }
        #endregion

        var mappedProdCategory = productCategoryMapper.DtoRegisterToDomain(dtoRegister);
        if (!await EntityValidationAsync(mappedProdCategory)) return false;

        var result = await productCategoryRepository.SaveAsync(mappedProdCategory);
        if (result)
        {
            GenerateLogger(ProductCategoryTrace.Save, userCredential.Id, mappedProdCategory.Id.ToString());
        }
        return true;
    }

    public async Task<bool> UpdateRegisterAsync(ProductCategoryUpdateRequest dtoUpdate, UserCredential userCredential)
    {
        #region Validation

        if (dtoUpdate.Name == null)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Update,
                EMessage.Required.GetDescription().FormatTo("Nome"));
            return false;
        }

        if (dtoUpdate.Id == 0)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Update,
                EMessage.Required.GetDescription().FormatTo("Id"));
            return false;
        }

        if (dtoUpdate.Description == null)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Update,
                EMessage.Required.GetDescription().FormatTo("Descrição"));
            return false;
        }

        if (dtoUpdate.Code == null)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Update,
                EMessage.Required.GetDescription().FormatTo("Code"));
            return false;
        }

        #endregion
        
        var productCategory = await productCategoryRepository.FindByPredicateAsync(x => x.Id == dtoUpdate.Id, asNoTracking: true);
        if (productCategory == null)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Delete,
                EMessage.NotFound.GetDescription().FormatTo("Id"));
            return false;
        }
        
        var updatedUser = productCategoryMapper.DtoUpdateToDomain(dtoUpdate, productCategory.Id);
        if(!await EntityValidationAsync(updatedUser)) return false;
        
        var result = await productCategoryRepository.UpdateAsync(updatedUser);
        if (result)
        {
            GenerateLogger(ProductCategoryTrace.Update, userCredential.Id, productCategory.Id.ToString());
        }
        return result;
    }

    public async Task<bool> DeleteRegisterAsync(ProductCategoryDeleteRequest dtoDelete, UserCredential userCredential)
    {
        if (dtoDelete.Id == 0)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Delete,
                EMessage.InvalidId.GetDescription().FormatTo("Id"));
            return false;
        }
        
        var productCategory = await productCategoryRepository.FindByPredicateAsync(x => x.Id == dtoDelete.Id);
        if (productCategory == null)
        {
            _notificationHandler.CreateNotification(
                ProductCategoryTrace.Delete,
                EMessage.UserNotFound.GetDescription());
            return false;
        }
        
        if(!await EntityValidationAsync(productCategory))  return false;
        
        var result = await productCategoryRepository.DeleteAsync(productCategory);
        if (result)
        {
            GenerateLogger(ProductCategoryTrace.Delete, userCredential.Id, productCategory.Id.ToString());
        }
        return result;
    }
}