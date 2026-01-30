using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;

namespace Teste_Xbits.ApplicationService.Services;

public abstract class ServiceBaseFacade(
    INotificationHandler notification,
    ILoggerHandler logger)
{
    protected readonly INotificationHandler Notification = notification;

    protected void GenerateLogger(
        string eventDescription,
        long userId,
        string? entityId = null)
    {
        var logger1 = new DomainLogger
        {
            ActionDate = DateTime.Now.GetDateAndTimeInBrasilia(),
            Description = eventDescription,
            UserId = userId,
            EntityId = entityId
        };

        logger.CreateLogger(logger1);
    }
}