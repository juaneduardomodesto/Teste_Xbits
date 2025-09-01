using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductRequest;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.ProductService;

public class ProductCommandService : ServiceBase<Product>, IProductCommandService
{
    private readonly INotificationHandler _notificationHandler;
    private readonly IValidate<Product> _validate;
    private readonly ILoggerHandler _loggerHandler;
    private readonly IProductMapper _productMapper;
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public ProductCommandService(
        INotificationHandler notification,
        IValidate<Product> validate,
        ILoggerHandler logger,
        IProductMapper productMapper,
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository
    ) : base(notification, validate, logger)
    {
        _notificationHandler = notification;
        _validate = validate;
        _loggerHandler = logger;
        _productMapper = productMapper;
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<bool> RegisterProductAsync(ProductRegisterRequest dtoRegister)
    {
        #region validation
        
        var preExist = await _productRepository.FindByPredicateAsync(
            x => x.Code == dtoRegister.Code);
        if (preExist != null)
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Exist.GetDescription().FormatTo("Produto"));
            return false;
        }

        if (string.IsNullOrEmpty(dtoRegister.Name))
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("Nome"));
            return false;
        }

        if (dtoRegister.Price <= 0)
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.InvalidMonetaryValue.GetDescription());
            return false;
        }

        if (string.IsNullOrEmpty(dtoRegister.Code))
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("Código do Produto"));
            return false;
        }

        if (dtoRegister is { HasExpirationDate: true, ExpirationDate: null })
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.Required.GetDescription().FormatTo("Data de Validade"));
            return false;
        }

        if (dtoRegister.ExpirationDate.HasValue && dtoRegister.ExpirationDate.Value < DateTime.Now)
        {
            _notificationHandler.CreateNotification(
                "Registro",
                EMessage.InvalidValue.GetDescription().FormatTo("Data de Validade"));
            return false;
        }
        
        if (dtoRegister.ProductCategoryId is > 0)
        {
            var productCategory = await _productCategoryRepository.FindByPredicateAsync(
                x => x.Id == dtoRegister.ProductCategoryId.Value);
    
            if (productCategory == null)
            {
                _notificationHandler.CreateNotification(
                    "Registro",
                    EMessage.CategoryNotFound.GetDescription());
                return false;
            }
        }

        #endregion

        var mappedProduct = _productMapper.DtoRegisterToDomain(dtoRegister);
        if (!await EntityValidationAsync(mappedProduct)) return false;

        _ = await _productRepository.SaveAsync(mappedProduct);
        return true;
    }

    public async Task<bool> UpdateProductAsync(ProductUpdateRequest dtoUpdate)
{
    #region Validation
    
    var existingProductWithCode = await _productRepository.FindByPredicateAsync(
        x => x.Code == dtoUpdate.Code && x.Id != dtoUpdate.ProductId);
    if (existingProductWithCode != null)
    {
        _notificationHandler.CreateNotification(
            "Atualização",
            EMessage.Exist.GetDescription().FormatTo("Código do Produto"));
        return false;
    }

    if (dtoUpdate.ProductId <= 0)
    {
        _notificationHandler.CreateNotification(
            "Atualização",
            EMessage.ProductNotFound.GetDescription());
        return false;
    }

    if (string.IsNullOrEmpty(dtoUpdate.Name))
    {
        _notificationHandler.CreateNotification(
            "Atualização",
            EMessage.Required.GetDescription().FormatTo("Nome"));
        return false;
    }

    if (dtoUpdate.Price <= 0)
    {
        _notificationHandler.CreateNotification(
            "Atualização",
            EMessage.InvalidMonetaryValue.GetDescription());
        return false;
    }

    if (string.IsNullOrEmpty(dtoUpdate.Code))
    {
        _notificationHandler.CreateNotification(
            "Atualização",
            EMessage.Required.GetDescription().FormatTo("Código do Produto"));
        return false;
    }

    if (dtoUpdate is { HasExpirationDate: true, ExpirationDate: null })
    {
        _notificationHandler.CreateNotification(
            "Atualização",
            EMessage.Required.GetDescription().FormatTo("Data de Validade"));
        return false;
    }

    if (dtoUpdate.ExpirationDate.HasValue && dtoUpdate.ExpirationDate.Value < DateTime.Now)
    {
        _notificationHandler.CreateNotification(
            "Atualização",
            EMessage.InvalidValue.GetDescription().FormatTo("Data de Validade"));
        return false;
    }
    
    if (dtoUpdate.ProductCategoryId is > 0)
    {
        var productCategory = await _productCategoryRepository.FindByPredicateAsync(
            x => x.Id == dtoUpdate.ProductCategoryId.Value);

        if (productCategory == null)
        {
            _notificationHandler.CreateNotification(
                "Atualização", 
                EMessage.CategoryNotFound.GetDescription());
            return false;
        }
    }

    #endregion
    
    var product = await _productRepository.FindByPredicateAsync(x => x.Id == dtoUpdate.ProductId, asNoTracking: true);
    if (product == null)
    {
        _notificationHandler.CreateNotification(
            "Atualização",
            EMessage.ProductNotFound.GetDescription());
        return false;
    }

    var updatedProduct = _productMapper.DtoUpdateToDomain(dtoUpdate, product.Id);
    if (!await EntityValidationAsync(updatedProduct)) return false;

    return await _productRepository.UpdateAsync(updatedProduct);
}

    public async Task<bool> DeleteProductAsync(ProductDeleteRequest dtoDelete)
    {
        #region Validation

        if (dtoDelete.ProductId <= 0)
        {
            _notificationHandler.CreateNotification(
                "Exclusão",
                EMessage.ProductNotFound.GetDescription());
            return false;
        }

        #endregion

        var product = await _productRepository.FindByPredicateAsync(x => x.Id == dtoDelete.ProductId);
        if (product == null)
        {
            _notificationHandler.CreateNotification(
                "Exclusão",
                EMessage.ProductNotFound.GetDescription());
            return false;
        }

        if (!await EntityValidationAsync(product)) return false;

        return await _productRepository.DeleteAsync(product);
    }
}