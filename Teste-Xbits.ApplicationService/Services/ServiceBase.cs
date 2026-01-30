using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Handlers.NotificationHandler;
using Teste_Xbits.Domain.Interface;

namespace Teste_Xbits.ApplicationService.Services;

public abstract class ServiceBase<T>(
    INotificationHandler notification,
    IValidate<T> validate,
    ILoggerHandler logger)
    where T : class
{
    protected readonly INotificationHandler Notification = notification;

    protected async Task<bool> EntityValidationAsync(T entity)
    {
        var validationResponse = await validate.ValidationAsync(entity);

        if (!validationResponse.Valid)
            Notification.CreateNotifications(DomainNotification.CreateNotifications(validationResponse.Errors));

        return validationResponse.Valid;
    }

    /// <summary>
    /// Generic method to validate any entity besides the main T entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity to be validated</typeparam>
    /// <param name="entity">Instance of the entity to be validated</param>
    /// <param name="validator">Specific validator for the entity</param>
    /// <returns>True if validation passed, False otherwise</returns>
    protected async Task<bool> EntityValidationAsync<TEntity>(TEntity entity, IValidate<TEntity> validator)
        where TEntity : class
    {
        var validationResponse = await validator.ValidationAsync(entity);

        if (!validationResponse.Valid)
            Notification.CreateNotifications(DomainNotification.CreateNotifications(validationResponse.Errors));

        return validationResponse.Valid;
    }

    protected bool EntitiesValidation(List<T> entities)
    {
        foreach (var validationResponse in entities
                     .Select(validate.Validation)
                     .Where(validationResponse => !validationResponse.Valid))
        {
            Notification.CreateNotifications(DomainNotification.CreateNotifications(validationResponse.Errors));
        }

        return !Notification.HasNotification();
    }

    /// <summary>
    /// Método genérico para validar lista de entidades de qualquer tipo
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade a ser validada</typeparam>
    /// <param name="entities">Lista de entidades a serem validadas</param>
    /// <param name="validator">Validador específico para a entidade</param>
    /// <returns>True se todas as validações passaram, False caso contrário</returns>
    protected bool EntitiesValidation<TEntity>(List<TEntity> entities, IValidate<TEntity> validator)
        where TEntity : class
    {
        foreach (var validationResponse in entities
                     .Select(validator.Validation)
                     .Where(validationResponse => !validationResponse.Valid))
        {
            Notification.CreateNotifications(DomainNotification.CreateNotifications(validationResponse.Errors));
        }

        return !Notification.HasNotification();
    }

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