using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;
using Teste_Xbits.Infra.Interfaces.ServiceContracts;
using Teste_Xbits.Infra.ORM.Context;
using Teste_Xbits.Infra.Repositories.Base;

namespace Teste_Xbits.Infra.Repositories;

public sealed class OrderRepository(
    ApplicationContext dbContext,
    IPaginationQueryService<Order> paginationQueryService)
    : RepositoryBase<Order>(dbContext), IOrderRepository
{
    private readonly ApplicationContext _dbContext = dbContext;

    public async Task<bool> SaveAsync(Order order)
    {
        await DbSetContext.AddAsync(order);
        return await SaveInDatabaseAsync();
    }

    public Task<bool> UpdateAsync(Order order)
    {
        DbSetContext.Update(order);
        return SaveInDatabaseAsync();
    }

    public Task<bool> DeleteAsync(Order order)
    {
        DetachedObject(order);
        DbSetContext.Remove(order);
        return SaveInDatabaseAsync();
    }

    public Task<Order?> FindByPredicateAsync(
        Expression<Func<Order, bool>> predicate,
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null,
        bool asNoTracking = false)
    {
        IQueryable<Order> query = DbSetContext;

        if (asNoTracking)
            query = query.AsNoTracking();

        if (include is not null)
            query = include(query);

        return query.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(predicate);
    }

    public Task<Order?> GetOrderWithItemsAsync(long orderId)
    {
        return DbSetContext
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public Task<PageList<Order>> FindAllWithPaginationAsync(
        PageParams pageParams,
        Expression<Func<Order, bool>>? predicate = null,
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null)
    {
        IQueryable<Order> query = DbSetContext;
        
        if (include is not null)
            query = include(query);
        
        if (predicate is not null)
            query = query.Where(predicate);
        
        query = query.OrderByDescending(o => o.CreatedAt);
        
        return paginationQueryService.CreatePaginationAsync(query, pageParams.PageSize, pageParams.PageNumber);
    }

    public async Task<bool> AddOrderItemAsync(OrderItem orderItem)
    {
        await _dbContext.Set<OrderItem>().AddAsync(orderItem);
        return await SaveInDatabaseAsync();
    }
}