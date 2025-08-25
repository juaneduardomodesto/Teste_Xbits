using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.Infra.Interfaces.RepositoryContracts;

public interface ITokenRepository
{
    Task<Token> SaveAsync(Token token);

    Task<bool> DeleteAsync(Token token);

    Task<bool> UpdateAsync(Token token);
    
    Task<Token?> FindByPredicateAsync(
        Expression<Func<Token, bool>> predicate,
        Func<IQueryable<Token>, IIncludableQueryable<Token, object>>? include = null,
        bool asNoTracking = false);

    Task<PageList<Token>> FindAllWithPaginationAsync(PageParams pageParams,
        Expression<Func<Token, bool>>? predicate = null, 
        Func<IQueryable<Token>, 
            IIncludableQueryable<Token, object>>? include = null);
}