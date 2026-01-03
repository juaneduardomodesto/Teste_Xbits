using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.Infra.Interfaces.RepositoryContracts;

public interface IOrderRepository
{
    Task<bool> SaveAsync(Order order);
    Task<bool> UpdateAsync(Order order);
    Task<bool> DeleteAsync(Order order);
    
    Task<Order?> FindByPredicateAsync(
        Expression<Func<Order, bool>> predicate,
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null,
        bool asNoTracking = false);
    
    Task<Order?> GetOrderWithItemsAsync(long orderId);
    
    Task<PageList<Order>> FindAllWithPaginationAsync(
        PageParams pageParams,
        Expression<Func<Order, bool>>? predicate = null,
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null);
    
    Task<bool> AddOrderItemAsync(OrderItem orderItem);
}