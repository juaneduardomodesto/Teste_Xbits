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

public sealed class UserRepository(
    ApplicationContext dbContext,
    IPaginationQueryService<User> paginationQueryService )
    : RepositoryBase<User>(dbContext), IUserRepository
{
    public async Task<User> SaveAsync(User user)
    {
        await DbSetContext.AddAsync(user);
        await SaveInDatabaseAsync();
        return user;
    }

    public Task<bool> DeleteAsync(User user)
    {
        DetachedObject(user);
        DbSetContext.Update(user);
        return SaveInDatabaseAsync();
    }

    public Task<bool> UpdateAsync(User user)
    {
        DbSetContext.Remove(user);
        return SaveInDatabaseAsync();
    }

    public Task<User?> FindByPredicateAsync(
        Expression<Func<User, bool>> predicate,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
        bool asNoTracking = false)
    {
        IQueryable<User> query = DbSetContext;
        if (asNoTracking)
            query = query.AsNoTracking();
        if (include is not null)
            query = include(query);
        return query.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(predicate);
    }

    public Task<PageList<User>> FindAllWithPaginationAsync(
        PageParams pageParams,
        Expression<Func<User, bool>>? predicate = null,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null)
    {
        IQueryable<User> query = DbSetContext;
        if (include is not null)
            query = include(query);
        if (predicate is not null)
            query = query.Where(predicate);
        query = query.OrderByDescending(f => f.CreatedAt);
        return paginationQueryService.CreatePaginationAsync(query, pageParams.PageSize, pageParams.PageNumber);
    }
}