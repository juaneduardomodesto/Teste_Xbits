using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.Infra.Interfaces.RepositoryContracts;

public interface IProductRepository
{
    Task<bool> SaveAsync(Product product);
    Task<bool> DeleteAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    public Task<Product?> FindByPredicateAsync(
        Expression<Func<Product, bool>> predicate,
        Func<IQueryable<Product>, IQueryable<Product>>? include = null,
        bool asNoTracking = false);
    Task<PageList<Product>> FindAllWithPaginationAsync(PageParams pageParams,
        Expression<Func<Product, bool>>? predicate = null, 
        Func<IQueryable<Product>, 
            IIncludableQueryable<Product, object>>? include = null);
}