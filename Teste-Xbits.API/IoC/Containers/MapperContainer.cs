using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Mappers.LoginMapper;
using Teste_Xbits.ApplicationService.Mappers.TokenMapper;
using Teste_Xbits.ApplicationService.Mappers.UserMapper;

namespace Teste_Xbits.API.IoC.Containers;

public static class MapperContainer
{
    public static IServiceCollection AddMapperContainer(this IServiceCollection services)
    {
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<ITokenMapper, TokenMapper>();
        services.AddScoped<ILoginMapper, LoginMapper>();
        return services;
    }
}