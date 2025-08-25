using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.Infra.Interfaces.RepositoryContracts;

public interface IUserRepository
{
    Task<User> SaveAsync(User user);

    Task<bool> DeleteAsync(User user);

    Task<bool> UpdateAsync(User user);
    
    public Task<User?> FindByPredicateAsync(
        Expression<Func<User, bool>> predicate,
        Func<IQueryable<User>, IQueryable<User>>? include = null,
        bool asNoTracking = false);
    
    Task<PageList<User>> FindAllWithPaginationAsync(PageParams pageParams,
        Expression<Func<User, bool>>? predicate = null, 
        Func<IQueryable<User>, 
            IIncludableQueryable<User, object>>? include = null);
}