using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.Infra.Interfaces.RepositoryContracts;

public interface ICartRepository
{
    Task<bool> SaveAsync(Cart cart);
    Task<bool> UpdateAsync(Cart cart);
    Task<bool> DeleteAsync(Cart cart);
    Task<Cart?> FindByPredicateAsync(
        Expression<Func<Cart, bool>> predicate,
        Func<IQueryable<Cart>, IIncludableQueryable<Cart, object>>? include = null,
        bool asNoTracking = false);
    Task<Cart> GetOrCreateActiveCartAsync(Guid userId);
    Task<Cart?> GetActiveCartWithItemsAsync(Guid userId);
    Task<Cart?> GetCartWithItemsAsync(long cartId);
    Task<bool> AddCartItemAsync(CartItem cartItem);
    Task<bool> UpdateCartItemAsync(CartItem cartItem);
    Task<bool> DeleteCartItemAsync(CartItem cartItem);
    Task<CartItem?> GetCartItemAsync(long cartItemId);
    Task<CartItem?> GetCartItemWithProductAsync(long cartItemId);
}