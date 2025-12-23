using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductRequest;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.ProductService;

public class ProductCommandService(
    INotificationHandler notification,
    IValidate<Product> validate,
    ILoggerHandler logger,
    IProductMapper productMapper,
    IProductRepository productRepository,
    IProductCategoryRepository productCategoryRepository)
    : ServiceBase<Product>(notification, validate, logger), IProductCommandService
{
    private readonly INotificationHandler _notificationHandler = notification;

    public async Task<bool> RegisterProductAsync(ProductRegisterRequest dtoRegister, UserCredential userCredential)
    {
        #region validation
        
        var preExist = await productRepository.FindByPredicateAsync(
            x => x.Code == dtoRegister.Code);
        if (preExist != null)
        {
            _notificationHandler.CreateNotification(
                ProductTrace.Save,
                EMessage.Exist.GetDescription().FormatTo("Produto"));
            return false;
        }

        if (string.IsNullOrEmpty(dtoRegister.Name))
        {
            _notificationHandler.CreateNotification(
                ProductTrace.Save,
                EMessage.Required.GetDescription().FormatTo("Nome"));
            return false;
        }

        if (dtoRegister.Price <= 0)
        {
            _notificationHandler.CreateNotification(
                ProductTrace.Save,
                EMessage.InvalidMonetaryValue.GetDescription());
            return false;
        }

        if (string.IsNullOrEmpty(dtoRegister.Code))
        {
            _notificationHandler.CreateNotification(
                ProductTrace.Save,
                EMessage.Required.GetDescription().FormatTo("Código do Produto"));
            return false;
        }

        if (dtoRegister is { HasExpirationDate: true, ExpirationDate: null })
        {
            _notificationHandler.CreateNotification(
                ProductTrace.Save,
                EMessage.Required.GetDescription().FormatTo("Data de Validade"));
            return false;
        }

        if (dtoRegister.ExpirationDate.HasValue && dtoRegister.ExpirationDate.Value < DateTime.Now)
        {
            _notificationHandler.CreateNotification(
                ProductTrace.Save,
                EMessage.InvalidValue.GetDescription().FormatTo("Data de Validade"));
            return false;
        }
        
        if (dtoRegister.ProductCategoryId is > 0)
        {
            var productCategory = await productCategoryRepository.FindByPredicateAsync(
                x => x.Id == dtoRegister.ProductCategoryId);
    
            if (productCategory == null)
            {
                _notificationHandler.CreateNotification(
                    ProductTrace.Save,
                    EMessage.CategoryNotFound.GetDescription());
                return false;
            }
        }

        #endregion

        var mappedProduct = productMapper.DtoRegisterToDomain(dtoRegister);
        if (!await EntityValidationAsync(mappedProduct)) return false;

        var result = await productRepository.SaveAsync(mappedProduct);
        if (result)
        {
            GenerateLogger(ProductTrace.Save, userCredential.Id, mappedProduct.Id.ToString());
        }
        return result;
    }

    public async Task<bool> UpdateProductAsync(ProductUpdateRequest dtoUpdate, UserCredential userCredential)
{
    #region Validation
    
    var existingProductWithCode = await productRepository.FindByPredicateAsync(
        x => x.Code == dtoUpdate.Code && x.Id != dtoUpdate.ProductId);
    if (existingProductWithCode != null)
    {
        _notificationHandler.CreateNotification(
            ProductTrace.Update,
            EMessage.Exist.GetDescription().FormatTo("Código do Produto"));
        return false;
    }

    if (dtoUpdate.ProductId <= 0)
    {
        _notificationHandler.CreateNotification(
            ProductTrace.Update,
            EMessage.ProductNotFound.GetDescription());
        return false;
    }

    if (string.IsNullOrEmpty(dtoUpdate.Name))
    {
        _notificationHandler.CreateNotification(
            ProductTrace.Update,
            EMessage.Required.GetDescription().FormatTo("Nome"));
        return false;
    }

    if (dtoUpdate.Price <= 0)
    {
        _notificationHandler.CreateNotification(
            ProductTrace.Update,
            EMessage.InvalidMonetaryValue.GetDescription());
        return false;
    }

    if (string.IsNullOrEmpty(dtoUpdate.Code))
    {
        _notificationHandler.CreateNotification(
            ProductTrace.Update,
            EMessage.Required.GetDescription().FormatTo("Código do Produto"));
        return false;
    }

    if (dtoUpdate is { HasExpirationDate: true, ExpirationDate: null })
    {
        _notificationHandler.CreateNotification(
            ProductTrace.Update,
            EMessage.Required.GetDescription().FormatTo("Data de Validade"));
        return false;
    }

    if (dtoUpdate.ExpirationDate.HasValue && dtoUpdate.ExpirationDate.Value < DateTime.Now)
    {
        _notificationHandler.CreateNotification(
            ProductTrace.Update,
            EMessage.InvalidValue.GetDescription().FormatTo("Data de Validade"));
        return false;
    }
    
    if (dtoUpdate.ProductCategoryId is > 0)
    {
        var productCategory = await productCategoryRepository.FindByPredicateAsync(
            x => x.Id == dtoUpdate.ProductCategoryId);

        if (productCategory == null)
        {
            _notificationHandler.CreateNotification(
                ProductTrace.Update,
                EMessage.CategoryNotFound.GetDescription());
            return false;
        }
    }

    #endregion
    
    var product = await productRepository.FindByPredicateAsync(x => x.Id == dtoUpdate.ProductId, asNoTracking: true);
    if (product == null)
    {
        _notificationHandler.CreateNotification(
            ProductTrace.Update,
            EMessage.ProductNotFound.GetDescription());
        return false;
    }

    var updatedProduct = productMapper.DtoUpdateToDomain(dtoUpdate, product.Id);
    if (!await EntityValidationAsync(updatedProduct)) return false;

    var result = await productRepository.UpdateAsync(updatedProduct);
    if (result)
    {
        GenerateLogger(ProductTrace.Update, userCredential.Id, updatedProduct.Id.ToString());
    }
    return result;
}

    public async Task<bool> DeleteProductAsync(ProductDeleteRequest dtoDelete, UserCredential userCredential)
    {
        #region Validation

        if (dtoDelete.ProductId <= 0)
        {
            _notificationHandler.CreateNotification(
                ProductTrace.Delete,
                EMessage.ProductNotFound.GetDescription());
            return false;
        }

        #endregion

        var product = await productRepository.FindByPredicateAsync(x => x.Id == dtoDelete.ProductId);
        if (product == null)
        {
            _notificationHandler.CreateNotification(
                "Exclusão",
                EMessage.ProductNotFound.GetDescription());
            return false;
        }

        var result = await productRepository.DeleteAsync(product);
        if (result)
        {
            GenerateLogger(ProductTrace.Delete, userCredential.Id, product.Id.ToString());
        }
        return result;
    }
}