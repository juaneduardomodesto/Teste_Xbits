using Teste_Xbits.API.IoC.Containers;
using Teste_Xbits.ApplicationService.Loggers;
using Teste_Xbits.Domain.Handlers.NotificationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.ORM.Context;
using Teste_Xbits.Infra.ORM.UoW;

namespace Teste_Xbits.API.IoC;

public static class InversionOfControlHandler
{
    public static void AddInversionOfControlHandler(this IServiceCollection services) =>
        services.AddScoped<ApplicationContext>()
            .AddScoped<INotificationHandler, NotificationHandler>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<ILoggerHandler, LoggerHandler>()
            .AddValidationContainer()
            .AddPaginationContainer()
            .AddMapperContainer()
            .AddServiceContainer()
            .AddRepositoryContainer();
}