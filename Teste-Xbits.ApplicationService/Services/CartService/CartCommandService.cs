using Teste_Xbits.ApplicationService.DataTransferObjects.Request.CartRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.CartResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.CartService;

public class CartCommandService(
    INotificationHandler notification,
    IValidate<Cart> validateCart,
    IValidate<CartItem> validateCartItem,
    ILoggerHandler logger,
    ICartRepository cartRepository,
    IProductRepository productRepository,
    ICartMapper cartMapper)
    : ServiceBase<Cart>(notification, validateCart, logger), ICartCommandService
{
    private readonly INotificationHandler _notificationHandler = notification;
    private readonly IValidate<CartItem> _validateCartItem = validateCartItem;

    public async Task<CartItemResponse?> AddToCartAsync(
        AddToCartRequest request,
        UserCredential userCredential)
    {
        #region Validations

        if (request.ProductId <= 0)
        {
            _notificationHandler.CreateNotification(
                CartTracer.AddItem,
                EMessage.InvalidId.GetDescription().FormatTo("ProductId"));
            return null;
        }

        if (request.Quantity <= 0)
        {
            _notificationHandler.CreateNotification(
                CartTracer.AddItem,
                "Quantidade deve ser maior que zero");
            return null;
        }

        #endregion
        
        var product = await productRepository.FindByPredicateAsync(x => x.Id == request.ProductId);
        if (product == null)
        {
            _notificationHandler.CreateNotification(
                CartTracer.AddItem,
                EMessage.NotFound.GetDescription().FormatTo("Produto"));
            return null;
        }
        
        var cart = await cartRepository.GetOrCreateActiveCartAsync(userCredential.Id);
        
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (existingItem != null)
        {
            var updatedItem = cartMapper.UpdateCartItemQuantity(existingItem, request.Quantity);
            
            if (!await EntityValidationAsync(updatedItem, _validateCartItem))
                return null;
            
            var result = await cartRepository.UpdateCartItemAsync(updatedItem);
            if(result)
                GenerateLogger(CartTracer.UpdateItem, userCredential.Id, updatedItem.Id.ToString());
            
            return cartMapper.CartItemToResponse(updatedItem);
        }
        else
        {
            var cartItem = cartMapper.CreateCartItemFromRequest(cart.Id, product, request.Quantity);
            
            if (!await EntityValidationAsync(cartItem, _validateCartItem))
                return null;
            
            var result = await cartRepository.AddCartItemAsync(cartItem);
            if(result)
                GenerateLogger(CartTracer.AddItem, userCredential.Id, cartItem.Id.ToString());
            
            var savedItem = await cartRepository.GetCartItemWithProductAsync(cartItem.Id);
            return savedItem != null ? cartMapper.CartItemToResponse(savedItem) : null;
        }
    }

    public async Task<bool> UpdateCartItemAsync(
        UpdateCartItemRequest request,
        UserCredential userCredential)
    {
        #region Validations

        if (request.CartItemId <= 0)
        {
            _notificationHandler.CreateNotification(
                CartTracer.UpdateItem,
                EMessage.InvalidId.GetDescription().FormatTo("CartItemId"));
            return false;
        }

        if (request.Quantity <= 0)
        {
            _notificationHandler.CreateNotification(
                CartTracer.UpdateItem,
                "Quantidade deve ser maior que zero");
            return false;
        }

        #endregion

        var cartItem = await cartRepository.GetCartItemAsync(request.CartItemId);
        if (cartItem == null)
        {
            _notificationHandler.CreateNotification(
                CartTracer.UpdateItem,
                EMessage.NotFound.GetDescription().FormatTo("Item do carrinho"));
            return false;
        }
        
        var cart = await cartRepository.FindByPredicateAsync(
            x => x.Id == cartItem.CartId && x.UserId == userCredential.Id);
        
        if (cart == null)
        {
            _notificationHandler.CreateNotification(
                CartTracer.UpdateItem,
                "Carrinho não encontrado ou não pertence ao usuário");
            return false;
        }
        
        var updatedItem = cartMapper.UpdateCartItemFromRequest(cartItem, request);
        if (!await EntityValidationAsync(updatedItem, _validateCartItem))
            return false;

        var result = await cartRepository.UpdateCartItemAsync(updatedItem);
        if (result)
        {
            GenerateLogger(CartTracer.UpdateItem, userCredential.Id, cartItem.Id.ToString());
        }

        return result;
    }

    public async Task<bool> RemoveFromCartAsync(
        RemoveFromCartRequest request,
        UserCredential userCredential)
    {
        #region Validations

        if (request.CartItemId <= 0)
        {
            _notificationHandler.CreateNotification(
                CartTracer.RemoveItem,
                EMessage.InvalidId.GetDescription().FormatTo("CartItemId"));
            return false;
        }

        #endregion

        var cartItem = await cartRepository.GetCartItemAsync(request.CartItemId);
        if (cartItem == null)
        {
            _notificationHandler.CreateNotification(
                CartTracer.RemoveItem,
                EMessage.NotFound.GetDescription().FormatTo("Item do carrinho"));
            return false;
        }
        
        var cart = await cartRepository.FindByPredicateAsync(
            x => x.Id == cartItem.CartId && x.UserId == userCredential.Id);
        
        if (cart == null)
        {
            _notificationHandler.CreateNotification(
                CartTracer.RemoveItem,
                "Carrinho não encontrado ou não pertence ao usuário");
            return false;
        }

        var result = await cartRepository.DeleteCartItemAsync(cartItem);
        if (result)
        {
            GenerateLogger(CartTracer.RemoveItem, userCredential.Id, cartItem.Id.ToString());
        }

        return result;
    }

    public async Task<bool> ClearCartAsync(UserCredential userCredential)
    {
        var cart = await cartRepository.GetActiveCartWithItemsAsync(userCredential.Id);
        if (cart == null)
            return true; // Nenhum carrinho para limpar

        foreach (var item in cart.Items)
        {
            await cartRepository.DeleteCartItemAsync(item);
        }

        GenerateLogger(CartTracer.ClearCart, userCredential.Id, cart.Id.ToString());
        return true;
    }
}