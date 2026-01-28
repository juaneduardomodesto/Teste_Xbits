using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;
using Teste_Xbits.Infra.ORM.Context;
using Teste_Xbits.Infra.Repositories.Base;

namespace Teste_Xbits.Infra.Repositories;

public sealed class CartRepository(ApplicationContext dbContext)
    : RepositoryBase<Cart>(dbContext), ICartRepository
{
    private readonly ApplicationContext _dbContext = dbContext;

    public async Task<bool> SaveAsync(Cart cart)
    {
        await DbSetContext.AddAsync(cart);
        return await SaveInDatabaseAsync();
    }

    public Task<bool> UpdateAsync(Cart cart)
    {
        DbSetContext.Update(cart);
        return SaveInDatabaseAsync();
    }

    public Task<bool> DeleteAsync(Cart cart)
    {
        DetachedObject(cart);
        DbSetContext.Remove(cart);
        return SaveInDatabaseAsync();
    }

    public Task<Cart?> FindByPredicateAsync(
        Expression<Func<Cart, bool>> predicate,
        Func<IQueryable<Cart>, IIncludableQueryable<Cart, object>>? include = null,
        bool asNoTracking = false)
    {
        IQueryable<Cart> query = DbSetContext;

        if (asNoTracking)
            query = query.AsNoTracking();

        if (include is not null)
            query = include(query);

        return query.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(predicate);
    }

    public async Task<Cart> GetOrCreateActiveCartAsync(Guid userId)
    {
        var cart = await DbSetContext
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == ECartStatus.Active);

        if (cart != null)
            return cart;

        var newCart = new Cart
        {
            UserId = userId,
            Status = ECartStatus.Active,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await DbSetContext.AddAsync(newCart);
        await SaveInDatabaseAsync();

        return newCart;
    }

    public Task<Cart?> GetActiveCartWithItemsAsync(Guid userId)
    {
        return DbSetContext
            .AsNoTracking() 
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == ECartStatus.Active);
    }

    public Task<Cart?> GetCartWithItemsAsync(long cartId)
    {
        return DbSetContext
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId);
    }
    
    public async Task<bool> AddCartItemAsync(CartItem cartItem)
    {
        await _dbContext.Set<CartItem>().AddAsync(cartItem);
        return await SaveInDatabaseAsync();
    }

    public Task<bool> UpdateCartItemAsync(CartItem cartItem)
    {
        _dbContext.Set<CartItem>().Update(cartItem);
        return SaveInDatabaseAsync();
    }

    public Task<bool> DeleteCartItemAsync(CartItem cartItem)
    {
        var entry = _dbContext.Entry(cartItem);
        if (entry.State == EntityState.Detached)
            _dbContext.Set<CartItem>().Attach(cartItem);
        
        _dbContext.Set<CartItem>().Remove(cartItem);
        return SaveInDatabaseAsync();
    }

    public Task<CartItem?> GetCartItemAsync(long cartItemId)
    {
        return _dbContext.Set<CartItem>().AsNoTracking()
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
    }

    public Task<CartItem?> GetCartItemWithProductAsync(long cartItemId)
    {
        return _dbContext.Set<CartItem>()
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
    }
}