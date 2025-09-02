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

public class ProductRepository(
    ApplicationContext dbContext,
    IPaginationQueryService<Product> paginationQueryService) 
    : RepositoryBase<Product>(dbContext), IProductRepository
{
    
    public async Task<bool> SaveAsync(Product product)
    {
        await DbSetContext.AddAsync(product);
        return await SaveInDatabaseAsync();
    }

    public Task<bool> DeleteAsync(Product product)
    {
        DetachedObject(product);
        DbSetContext.Remove(product);
        return SaveInDatabaseAsync();
    }

    public Task<bool> UpdateAsync(Product product)
    {
        DbSetContext.Update(product);
        return SaveInDatabaseAsync();
    }

    public Task<Product?> FindByPredicateAsync(Expression<Func<Product, 
        bool>> predicate, Func<IQueryable<Product>, IQueryable<Product>>? 
        include = null, bool asNoTracking = false)
    {   
        IQueryable<Product> query = DbSetContext;
        if (asNoTracking)
            
            query = query.AsNoTracking();
        
        if (include is not null)
            query = include(query);
        return query.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(predicate);
    }

    public Task<PageList<Product>> FindAllWithPaginationAsync(PageParams pageParams, Expression<Func<Product, bool>>? 
        predicate = null, Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null)
    {
        IQueryable<Product> query = DbSetContext;
        if (include is not null)
            query = include(query);
        if (predicate is not null)
            query = query.Where(predicate);
        query = query.OrderByDescending(f => f.CreatedAt);
        return paginationQueryService.CreatePaginationAsync(query, pageParams.PageSize, pageParams.PageNumber);
    }
}