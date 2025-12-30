using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.ApplicationService.Traces;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;
using Teste_Xbits.Infra.Interfaces.ServiceContracts;

namespace Teste_Xbits.ApplicationService.Services.ImageService;

public class ImageCommandService(
    INotificationHandler notification,
    IValidate<ImageFiles> validate,
    ILoggerHandler logger,
    IImageRepository imageRepository,
    IImageStorageService storageService,
    IImageResizerService imageResizer,
    IImageMapper imageMapper)
    : ServiceBase<ImageFiles>(notification, validate, logger), IImageCommandService
{
    private readonly INotificationHandler _notificationHandler = notification;

    public async Task<ImageResponse?> UploadImageAsync(
        ImageRegisterRequest request,
        UserCredential userCredential)
    {
        #region Validations

        if (request.File.Length == 0)
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Save,
                EMessage.Required.GetDescription().FormatTo("Arquivo de imagem"));
            return null;
        }

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/jpg" };
        if (!allowedTypes.Contains(request.File.ContentType.ToLower()))
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Save,
                "Tipo de imagem não suportado. Use JPEG, PNG ou WebP");
            return null;
        }

        const long maxSize = 5 * 1024 * 1024; // 5MB
        if (request.File.Length > maxSize)
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Save,
                "Imagem muito grande. Tamanho máximo: 5MB");
            return null;
        }

        if (request.EntityId <= 0)
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Save,
                EMessage.InvalidId.GetDescription().FormatTo("EntityId"));
            return null;
        }

        #endregion

        try
        {
            await using var originalStream = request.File.OpenReadStream();
            var uploadResult = await storageService.UploadAsync(
                originalStream,
                request.File.FileName,
                request.File.ContentType);

            // Create THUMBNAIL (150x150)
            originalStream.Position = 0;
            var thumbnailStream = await imageResizer.CreateThumbnailAsync(originalStream, 150);
            var thumbnailFileName = $"thumb_{request.File.FileName}";
            var thumbnailResult = await storageService.UploadAsync(
                thumbnailStream,
                thumbnailFileName,
                request.File.ContentType);

            // Create MEDIUM (800x800)
            originalStream.Position = 0;
            var mediumStream = await imageResizer.ResizeImageAsync(originalStream, 800, 800);
            var mediumFileName = $"medium_{request.File.FileName}";
            var mediumResult = await storageService.UploadAsync(
                mediumStream,
                mediumFileName,
                request.File.ContentType);

            // Create LARGE (1920x1920)
            originalStream.Position = 0;
            var largeStream = await imageResizer.ResizeImageAsync(originalStream, 1920, 1920);
            var largeFileName = $"large_{request.File.FileName}";
            var largeResult = await storageService.UploadAsync(
                largeStream,
                largeFileName,
                request.File.ContentType);
            
            var image = imageMapper.UploadRequestToDomain(
                request,
                uploadResult,
                thumbnailResult,
                mediumResult,
                largeResult);

            if (!await EntityValidationAsync(image))
            {
                await storageService.DeleteAsync(uploadResult.StoragePath);
                await storageService.DeleteAsync(thumbnailResult.StoragePath);
                await storageService.DeleteAsync(mediumResult.StoragePath);
                await storageService.DeleteAsync(largeResult.StoragePath);
                return null;
            }
            
            var saved = await imageRepository.SaveAsync(image);
            if (!saved)
            {
                await storageService.DeleteAsync(uploadResult.StoragePath);
                await storageService.DeleteAsync(thumbnailResult.StoragePath);
                await storageService.DeleteAsync(mediumResult.StoragePath);
                await storageService.DeleteAsync(largeResult.StoragePath);
                return null;
            }
            
            if (request.SetAsMain)
            {
                await imageRepository.UnsetMainImagesAsync(
                    request.EntityType,
                    request.EntityId,
                    image.Id);
            }

            GenerateLogger(ImageTracer.Save, userCredential.Id, image.Id.ToString());
            
            var savedImage = await imageRepository.FindByPredicateAsync(x => x.StoragePath == image.StoragePath);
            
            await thumbnailStream.DisposeAsync();
            await mediumStream.DisposeAsync();
            await largeStream.DisposeAsync();

            return savedImage != null ? imageMapper.DomainToResponse(savedImage) : null;
        }
        catch (Exception ex)
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Save,
                $"Erro ao fazer upload da imagem: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteImageAsync(
        ImageDeleteRequest request,
        UserCredential userCredential)
    {
        #region Validations

        if (request.Id <= 0)
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Delete,
                EMessage.InvalidId.GetDescription().FormatTo("Id"));
            return false;
        }

        #endregion

        var image = await imageRepository.FindByPredicateAsync(x => x.Id == request.Id);
        if (image == null)
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Delete,
                EMessage.NotFound.GetDescription().FormatTo("Imagem"));
            return false;
        }
        
        await storageService.DeleteAsync(image.StoragePath); // Original
        
        // Delete all versions
        if (!string.IsNullOrEmpty(image.ThumbnailUrl))
        {
            var thumbnailPath = ExtractPathFromUrl(image.ThumbnailUrl);
            await storageService.DeleteAsync(thumbnailPath);
        }
        
        if (!string.IsNullOrEmpty(image.MediumUrl))
        {
            var mediumPath = ExtractPathFromUrl(image.MediumUrl);
            await storageService.DeleteAsync(mediumPath);
        }
        
        if (!string.IsNullOrEmpty(image.LargeUrl))
        {
            var largePath = ExtractPathFromUrl(image.LargeUrl);
            await storageService.DeleteAsync(largePath);
        }

        // Delete on db
        var result = await imageRepository.DeleteAsync(image);
        if (result)
        {
            GenerateLogger(ImageTracer.Delete, userCredential.Id, image.Id.ToString());
        }

        return result;
    }

    public async Task<bool> SetMainImageAsync(
        ImageSetMainRequest request,
        UserCredential userCredential)
    {
        #region Validations

        if (request.ImageId <= 0)
        {
            _notificationHandler.CreateNotification(
                ImageTracer.SetMain,
                EMessage.InvalidId.GetDescription().FormatTo("ImageId"));
            return false;
        }

        #endregion

        var image = await imageRepository.FindByPredicateAsync(
            x => x.Id == request.ImageId, 
            asNoTracking: true);
        if (image == null)
        {
            _notificationHandler.CreateNotification(
                ImageTracer.SetMain,
                EMessage.NotFound.GetDescription().FormatTo("Imagem"));
            return false;
        }

        var result = await imageRepository.SetMainImageAsync(
            request.ImageId,
            image.EntityType,
            image.EntityId);

        if (result)
        {
            GenerateLogger(ImageTracer.SetMain, userCredential.Id, image.Id.ToString());
        }

        return result;
    }

    public async Task<bool> UpdateImageOrderAsync(
        ImageUpdateOrderRequest request,
        UserCredential userCredential)
    {
        #region Validations

        if (request.Items == null || !request.Items.Any())
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Update,
                "Lista de itens não pode estar vazia");
            return false;
        }

        #endregion

        foreach (var item in request.Items)
        {
            var image = await imageRepository.FindByPredicateAsync(x => x.Id == item.ImageId, asNoTracking: true);
            if (image == null) continue;

            var updatedImage = image with
            {
                DisplayOrder = item.DisplayOrder,
                UpdatedAt = DateTime.Now
            };

            await imageRepository.UpdateAsync(updatedImage);
        }

        GenerateLogger(ImageTracer.Update, userCredential.Id, "Update Order");
        return true;
    }

    public async Task<bool> UpdateImageAltAsync(
        ImageUpdateAltRequest request,
        UserCredential userCredential)
    {
        #region Validations

        if (request.ImageId <= 0)
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Update,
                EMessage.InvalidId.GetDescription().FormatTo("ImageId"));
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Alt))
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Update,
                "Texto alternativo não pode estar vazio");
            return false;
        }

        #endregion

        var image = await imageRepository.FindByPredicateAsync(x => x.Id == request.ImageId, asNoTracking: true);
        if (image == null)
        {
            _notificationHandler.CreateNotification(
                ImageTracer.Update,
                EMessage.NotFound.GetDescription().FormatTo("Imagem"));
            return false;
        }

        var updatedImage = image with
        {
            Alt = request.Alt,
            UpdatedAt = DateTime.Now
        };

        var result = await imageRepository.UpdateAsync(updatedImage);
        if (result)
        {
            GenerateLogger(ImageTracer.Update, userCredential.Id, image.Id.ToString());
        }

        return result;
    }

    public async Task<bool> DeleteEntityImagesAsync(
        EEntityType entityType,
        long entityId,
        UserCredential userCredential)
    {
        var images = await imageRepository.FindByEntityAsync(entityType, entityId);

        foreach (var image in images)
        {
            //Delete all versions
            await storageService.DeleteAsync(image.StoragePath);
            
            if (!string.IsNullOrEmpty(image.ThumbnailUrl))
                await storageService.DeleteAsync(ExtractPathFromUrl(image.ThumbnailUrl));
            
            if (!string.IsNullOrEmpty(image.MediumUrl))
                await storageService.DeleteAsync(ExtractPathFromUrl(image.MediumUrl));
            
            if (!string.IsNullOrEmpty(image.LargeUrl))
                await storageService.DeleteAsync(ExtractPathFromUrl(image.LargeUrl));

            //Delete on db
            await imageRepository.DeleteAsync(image);
        }

        GenerateLogger(ImageTracer.Delete, userCredential.Id,
            $"Deleted all images for {entityType}:{entityId}");

        return true;
    }

    public async Task<Stream?> DownloadImageAsync(long imageId)
    {
        var image = await imageRepository.FindByPredicateAsync(x => x.Id == imageId);
        if (image == null) return null;

        return await storageService.DownloadAsync(image.StoragePath);
    }

    // Helper method to extract URL path
    private string ExtractPathFromUrl(string url)
    {
        // Remove the base URL to get the path
        // ex: "/uploads/2024/12/thumb_image.jpg" -> "2024/12/thumb_image.jpg"
        if (url.StartsWith("/uploads/"))
        {
            return url.Replace("/uploads/", "");
        }
        return url;
    }
}