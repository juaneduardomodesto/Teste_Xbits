using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.ApplicationService.Services.LoginService;
using Teste_Xbits.ApplicationService.Services.Register;
using Teste_Xbits.ApplicationService.Services.TokenService;

namespace Teste_Xbits.API.IoC.Containers;

public static class ServiceContainer
{
    public static IServiceCollection AddServiceContainer(this IServiceCollection services)
    {
        services.AddScoped<ILoginQueryService, LoginQueryService>();
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<IUserQueryService,  UserQueryService>();
        services.AddScoped<ITokenCommandService, TokenCommandCommandService>();
        return services;
    }
}