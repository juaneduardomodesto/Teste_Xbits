using Teste_Xbits.ApplicationService.DataTransferObjects.Request.CartRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.CartResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.Mappers.CartMapper;

public class CartMapper : ICartMapper
{
    public CartResponse DomainToResponse(Cart cart)
    {
        return new CartResponse
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Status = cart.Status,
            Subtotal = cart.Subtotal,
            TotalItems = cart.TotalItems,
            Items = cart.Items.Select(CartItemToResponse).ToList(),
            CreatedAt = cart.CreatedAt
        };
    }

    public CartItemResponse CartItemToResponse(CartItem cartItem)
    {
        return new CartItemResponse
        {
            Id = cartItem.Id,
            ProductId = cartItem.ProductId,
            ProductName = cartItem.Product?.Name ?? "Produto",
            ProductCode = cartItem.Product?.Code ?? string.Empty,
            Quantity = cartItem.Quantity,
            UnitPrice = cartItem.UnitPrice,
            TotalPrice = cartItem.TotalPrice,
            ProductImageUrl = null
        };
    }

    public CartItem CreateCartItemFromRequest(long cartId, Product product, int quantity)
    {
        return new CartItem
        {
            CartId = cartId,
            ProductId = product.Id,
            Quantity = quantity,
            UnitPrice = product.Price,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public CartItem UpdateCartItemQuantity(CartItem existingItem, int additionalQuantity)
    {
        return existingItem with
        {
            Quantity = existingItem.Quantity + additionalQuantity,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public CartItem UpdateCartItemFromRequest(CartItem cartItem, UpdateCartItemRequest request)
    {
        return cartItem with
        {
            Quantity = request.Quantity,
            UpdatedAt = DateTime.UtcNow
        };
    }
}