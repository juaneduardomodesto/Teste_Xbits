using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.Infra.Interfaces.RepositoryContracts;

public interface IProductCategoryRepository
{
    Task<bool> SaveAsync(ProductCategory dtoRegister);
    Task<bool> DeleteAsync(ProductCategory dtoUpdate);
    Task<bool> UpdateAsync(ProductCategory dtoDelete);
    public Task<ProductCategory?> FindByPredicateAsync(
        Expression<Func<ProductCategory, bool>> predicate,
        Func<IQueryable<ProductCategory>, IQueryable<ProductCategory>>? include = null,
        bool asNoTracking = false);
    Task<PageList<ProductCategory>> FindAllWithPaginationAsync(PageParams pageParams,
        Expression<Func<ProductCategory, bool>>? predicate = null, 
        Func<IQueryable<ProductCategory>, 
            IIncludableQueryable<ProductCategory, object>>? include = null);
}