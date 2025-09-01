using System.Linq.Expressions;
using LinqKit;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.UserResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.UserService;

public class UserQueryService(
    INotificationHandler notification,
    IValidate<User> validate,
    ILoggerHandler logger,
    IUserRepository userRepository,
    IUserMapper userMapper)
    : ServiceBase<User>(notification, validate, logger), IUserQueryService
{

    public async Task<UserResponse?> FindByIdAsync(long id)
    {
        var user = await userRepository.FindByPredicateAsync(x => x.Id == id);

        return user is null
            ? null
            : userMapper.DomainToSimpleResponse(user);
    }

    public async Task<PageList<UserResponse>> FindAllWithPaginationAsync(
        string? namePrefix,
        string? emailPrefix,
        string? cpfPrefix,
        PageParams pageParams)
    {
        try
        {
            Expression<Func<User, bool>> predicate = PredicateBuilder.New<User>(x => true);

            if (!string.IsNullOrEmpty(namePrefix))
            {
                predicate = predicate.And(e =>
                    e.Name.StartsWith(namePrefix));
            }

            if (!string.IsNullOrEmpty(emailPrefix))
            {
                predicate = predicate.And(e =>
                    e.Email.StartsWith(emailPrefix));
            }

            if (!string.IsNullOrEmpty(cpfPrefix))
            {
                predicate = predicate.And(e =>
                    e.Cpf.StartsWith(cpfPrefix));
            }

            var userList = await userRepository.FindAllWithPaginationAsync(
                pageParams,
                predicate
            );

            return !userList.Items.Any()
                ? new PageList<UserResponse>()
                : userMapper.DomainToPaginationUserResponse(userList);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}