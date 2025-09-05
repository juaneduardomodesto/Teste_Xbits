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

public class ProductCategoryRepository(
    ApplicationContext dbContext,
    IPaginationQueryService<ProductCategory> paginationQueryService) 
    : RepositoryBase<ProductCategory>(dbContext), IProductCategoryRepository
{

    public async Task<bool> SaveAsync(ProductCategory dtoRegister)
    {
        await DbSetContext.AddAsync(dtoRegister);
        return await SaveInDatabaseAsync();
    }

    public Task<bool> DeleteAsync(ProductCategory dtoupdate)
    {
        DetachedObject(dtoupdate);
        DbSetContext.Remove(dtoupdate);
        return SaveInDatabaseAsync();
    }

    public Task<bool> UpdateAsync(ProductCategory dtoDelete)
    {
        DbSetContext.Update(dtoDelete);
        return  SaveInDatabaseAsync();
    }

    public Task<ProductCategory?> FindByPredicateAsync(Expression<Func<ProductCategory, bool>> predicate, 
        Func<IQueryable<ProductCategory>, IQueryable<ProductCategory>>? include = null, bool asNoTracking = false)
    {
        IQueryable<ProductCategory> query = DbSetContext;

        if (asNoTracking)
            query = query.AsNoTracking();

        if (include is not null)
            query = include(query);

        return query.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(predicate);
    }

    public Task<PageList<ProductCategory>> FindAllWithPaginationAsync(PageParams pageParams, 
        Expression<Func<ProductCategory, bool>>? predicate = null, 
        Func<IQueryable<ProductCategory>, IQueryable<ProductCategory>>? include = null) // Mudança aqui
    {
        IQueryable<ProductCategory> query = DbSetContext;
        if (include is not null)
            query = include(query);
        if (predicate is not null)
            query = query.Where(predicate);
        query = query.OrderByDescending(f => f.CreatedAt);
        return paginationQueryService.CreatePaginationAsync(query, pageParams.PageSize, pageParams.PageNumber);
    }
}