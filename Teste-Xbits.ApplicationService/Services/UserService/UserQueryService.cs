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

public class UserQueryService : ServiceBase<User>, IUserQueryService
{
    private readonly INotificationHandler notificationHandler;
    private readonly ILoggerHandler logger;
    private readonly IValidate<User> validate;
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;

    public UserQueryService(
        INotificationHandler notification,
        IValidate<User> validate,
        ILoggerHandler logger,
        IUserRepository userRepository,
        IUserMapper userMapper)
        : base(notification, validate, logger)
    {
        this.notificationHandler = notification;
        this.validate = validate;
        this.logger = logger;
        this._userRepository  = userRepository;
        this._userMapper = userMapper;
    }

    public async Task<UserResponse?> FindByIdAsync(long id)
    {
        var user = await _userRepository.FindByPredicateAsync(x => x.Id == id);
        //Todo: connect products

        return user is null
            ? null
            : _userMapper.DomainToSimpleResponse(user);
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
                    e.Name != null &&
                    e.Name.StartsWith(namePrefix));
            }

            if (!string.IsNullOrEmpty(emailPrefix))
            {
                predicate = predicate.And(e =>
                    e.Email != null &&
                    e.Email.StartsWith(emailPrefix));
            }

            if (!string.IsNullOrEmpty(cpfPrefix))
            {
                predicate = predicate.And(e =>
                    e.Cpf != null &&
                    e.Cpf.StartsWith(cpfPrefix));
            }

            var userList = await _userRepository.FindAllWithPaginationAsync(
                pageParams,
                predicate
            );

            return !userList.Items.Any()
                ? new PageList<UserResponse>()
                : _userMapper.DomainToPaginationUserResponse(userList);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}