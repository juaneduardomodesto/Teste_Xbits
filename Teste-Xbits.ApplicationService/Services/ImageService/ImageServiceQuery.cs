using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.ImageService;

public class ImageQueryService(
    INotificationHandler notification,
    IValidate<ImageFiles> validate,
    ILoggerHandler logger,
    IImageRepository imageRepository,
    IImageMapper imageMapper)
    : ServiceBase<ImageFiles>(notification, validate, logger), IImageQueryService
{
    public async Task<ImageResponse?> FindByIdAsync(long id)
    {
        var image = await imageRepository.FindByPredicateAsync(x => x.Id == id);
        return image != null ? imageMapper.DomainToResponse(image) : null;
    }

    public async Task<IEnumerable<ImageResponse>> FindByEntityAsync(
        EEntityType entityType,
        long entityId)
    {
        var images = await imageRepository.FindByEntityAsync(entityType, entityId);
        return imageMapper.DomainListToResponseList(images);
    }

    /// <summary>
    /// Busca a imagem principal de uma entidade específica
    /// Retorna null se não houver imagem principal definida
    /// </summary>
    /// <param name="entityType">Tipo da entidade (User, Product, etc)</param>
    /// <param name="entityId">ID da entidade</param>
    /// <returns>ImageResponse com todas as URLs ou null</returns>
    public async Task<ImageResponse?> GetMainImageAsync(
        EEntityType entityType,
        long entityId)
    {
        var mainImage = await imageRepository.GetMainImageAsync(entityType, entityId);
        
        if (mainImage != null)
        {
            return imageMapper.DomainToResponse(mainImage);
        }

        // Fallback
        var images = await imageRepository.FindByEntityAsync(entityType, entityId);
        var firstImage = images.OrderBy(i => i.DisplayOrder)
            .ThenByDescending(i => i.CreatedAt)
            .FirstOrDefault();

        return firstImage != null ? imageMapper.DomainToResponse(firstImage) : null;
    }
}