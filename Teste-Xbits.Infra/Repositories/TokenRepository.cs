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

public sealed class TokenRepository(
    ApplicationContext dbContext,
    IPaginationQueryService<Token> paginationQueryService)
    : RepositoryBase<Token>(dbContext), ITokenRepository
{
    public async Task<Token> SaveAsync(Token token)
    {
        await DbSetContext.AddAsync(token);
        await SaveInDatabaseAsync();
        return token;
    }

    public Task<bool> DeleteAsync(Token token)
    {
        DetachedObject(token);
        DbSetContext.Update(token);
        return SaveInDatabaseAsync();
    }

    public Task<bool> UpdateAsync(Token token)
    {
        DbSetContext.Remove(token);
        return SaveInDatabaseAsync();
    }

    public Task<Token?> FindByPredicateAsync(Expression<Func<Token, bool>> predicate,
        Func<IQueryable<Token>, IIncludableQueryable<Token,
            object>>? include = null, bool asNoTracking = false)
    {
        IQueryable<Token> query = DbSetContext;
        if (asNoTracking)
            query = query.AsNoTracking();
        if (include is not null)
            query = include(query);
        return query.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(predicate);
    }

    public Task<PageList<Token>> FindAllWithPaginationAsync(PageParams pageParams,
        Expression<Func<Token, bool>>? predicate = null, Func<IQueryable<Token>,
            IIncludableQueryable<Token, object>>? include = null)
    {
        IQueryable<Token> query = DbSetContext;
        if (include is not null)
            query = include(query);
        if (predicate is not null)
            query = query.Where(predicate);
        query = query.OrderByDescending(f => f.CreatedAt);
        return paginationQueryService.CreatePaginationAsync(query, pageParams.PageSize, pageParams.PageNumber);
    }
}