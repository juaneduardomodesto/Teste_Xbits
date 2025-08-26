using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.ProductCategoryService;

public class ProductCategoryCommandService : ServiceBase<ProductCategory>, IProductCategoryCommandService
{
    private readonly INotificationHandler _notificationHandler;
    private readonly IValidate<ProductCategory> _validate;
    private readonly ILoggerHandler _loggerHandler;
    private readonly IProductCategoryMapper _productCategoryMapper;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public ProductCategoryCommandService(
        INotificationHandler notification,
        IValidate<ProductCategory> validate,
        ILoggerHandler logger,
        IProductCategoryMapper productCategoryMapper,
        IProductCategoryRepository productCategoryRepository)
        : base(notification, validate, logger)
    {
        this._notificationHandler = notification;
        this._validate = validate;
        this._loggerHandler = logger;
        this._productCategoryMapper = productCategoryMapper;
        this._productCategoryRepository = productCategoryRepository;
    }

    public async Task<bool> RegisterAsync(ProductCategoryRegisterRequest dtoRegister)
    {
        #region Validation

        if (dtoRegister.Name == null)
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.Required.GetDescription().FormatTo("Nome"));
            return false;
        }
        
        var preExist = await _productCategoryRepository.FindByPredicateAsync(
            x =>x.ProductCategoryCode == dtoRegister.Code);
        if (preExist != null)
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Exist.GetDescription().FormatTo("Usuário"));
            return false;
        }

        if (dtoRegister.Description == null)
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.Required.GetDescription().FormatTo("Descrição"));
            return false;
        }

        if (dtoRegister.Code == null)
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.Required.GetDescription().FormatTo("Code"));
            return false;
        }
        #endregion

        var mappedProdCategory = _productCategoryMapper.DtoRegisterToDomain(dtoRegister);
        if (!await EntityValidationAsync(mappedProdCategory)) return false;

        var productCategory = await _productCategoryRepository.SaveAsync(mappedProdCategory);
        return true;
    }

    public async Task<bool> UpdateRegisterAsync(ProductCategoryUpdateRequest dtoUpdate)
    {
        #region Validation

        if (dtoUpdate.Name == null)
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.Required.GetDescription().FormatTo("Nome"));
            return false;
        }

        if (dtoUpdate.Id == null)
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.Required.GetDescription().FormatTo("Id"));
            return false;
        }

        if (dtoUpdate.Description == null)
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.Required.GetDescription().FormatTo("Descrição"));
            return false;
        }

        if (dtoUpdate.Code == null)
        {
            _notificationHandler.CreateNotification(
                "Login",
                EMessage.Required.GetDescription().FormatTo("Code"));
            return false;
        }

        #endregion
        
        var productCategory = await _productCategoryRepository.FindByPredicateAsync(x => x.Id == dtoUpdate.Id, asNoTracking: true);
        if (productCategory == null)
        {
            _notificationHandler.CreateNotification(
                "Delete",
                EMessage.NotFound.GetDescription().FormatTo("Id"));
            return false;
        }
        
        var updatedUser = _productCategoryMapper.DtoUpdateToDomain(dtoUpdate, productCategory.Id);
        if(!await EntityValidationAsync(updatedUser)) return false;
        
        return await _productCategoryRepository.UpdateAsync(updatedUser);
    }

    public async Task<bool> DeleteRegisterAsync(ProductCategoryDeleteRequest dtoDelete)
    {
        if (dtoDelete.Id == null)
        {
            _notificationHandler.CreateNotification(
                "Delete",
                EMessage.InvalidId.GetDescription().FormatTo("Id"));
            return false;
        }
        
        var productCategory = await _productCategoryRepository.FindByPredicateAsync(x => x.Id == dtoDelete.Id);
        if (productCategory == null)
        {
            _notificationHandler.CreateNotification(
                "Delete",
                EMessage.UserNotFound.GetDescription());
            return false;
        }
        
        if(!await EntityValidationAsync(productCategory!))  return false;
        
        return await _productCategoryRepository.DeleteAsync(productCategory!);
    }
}