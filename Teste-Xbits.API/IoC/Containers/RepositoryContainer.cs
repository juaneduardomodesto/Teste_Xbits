using Teste_Xbits.Infra.Interfaces.RepositoryContracts;
using Teste_Xbits.Infra.Repositories;

namespace Teste_Xbits.API.IoC.Containers;

public static class RepositoryContainer
{
    public static IServiceCollection AddRepositoryContainer(this IServiceCollection services)
    {
        services.AddScoped<IDomainLoggerRepository, DomainLoggerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        return services;
    }
}