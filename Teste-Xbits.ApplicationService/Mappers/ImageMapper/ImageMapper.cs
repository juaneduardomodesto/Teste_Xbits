using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.Interfaces.ServiceContracts;

namespace Teste_Xbits.ApplicationService.Mappers.ImageMapper;

public class ImageMapper : IImageMapper
{
    public ImageResponse DomainToResponse(ImageFiles image)
    {
        return new ImageResponse
        {
            Id = image.Id,
            FileName = image.FileName,
            ContentType = image.ContentType,
            SizeInBytes = image.SizeInBytes,
            ImageType = image.ImageType,
            IsMain = image.IsMain,
            Alt = image.Alt,
            DisplayOrder = image.DisplayOrder,
            // URLs for sizes
            OriginalUrl = image.OriginalUrl,
            ThumbnailUrl = image.ThumbnailUrl,
            MediumUrl = image.MediumUrl,
            LargeUrl = image.LargeUrl,
            
            CreatedAt = image.CreatedAt
        };
    }

    public IEnumerable<ImageResponse> DomainListToResponseList(IEnumerable<ImageFiles> images)
    {
        return images.Select(DomainToResponse).ToList();
    }

    /// <summary>
    /// Helper method to convert a multiple upload request to individual upload requests
    /// </summary>
    /// <param name="multipleRequest">The multiple upload request containing multiple files</param>
    /// <returns>A list of individual upload requests ready for processing</returns>
    /// <remarks>
    /// This method splits a multi-file upload request into individual requests,
    /// optionally setting the first file as main image if specified.
    /// </remarks>
    public IEnumerable<ImageRegisterRequest> MultipleToIndividualRequests(
        ImageMultipleUploadRequest multipleRequest)
    {
        return multipleRequest.Files.Select((t, i) => new ImageRegisterRequest()
            {
                File = t,
                EntityType = multipleRequest.EntityType,
                EntityId = multipleRequest.EntityId,
                ImageType = multipleRequest.ImageType,
                SetAsMain = i == 0 && multipleRequest.SetFirstAsMain,
                Alt = multipleRequest.Alts?.ElementAtOrDefault(i),
            })
            .ToList();
    }

    /// <summary>
    /// Converts an upload request to an ImageFiles domain entity
    /// </summary>
    /// <param name="request">The image registration request</param>
    /// <param name="uploadResult">Result of the original image upload</param>
    /// <param name="thumbnailResult">Result of the thumbnail upload</param>
    /// <param name="mediumResult">Result of the medium size upload</param>
    /// <param name="largeResult">Result of the large size upload</param>
    /// <returns>An ImageFiles domain entity populated with all data</returns>
    public ImageFiles UploadRequestToDomain(
        ImageRegisterRequest request,
        ImageUploadResult uploadResult,
        ImageUploadResult thumbnailResult,
        ImageUploadResult mediumResult,
        ImageUploadResult largeResult)
    {
        return new ImageFiles
        {
            FileName = request.File.FileName,
            StoragePath = uploadResult.StoragePath,
            ContentType = request.File.ContentType,
            SizeInBytes = uploadResult.SizeInBytes,
            ImageType = request.ImageType,
            EntityType = request.EntityType,
            EntityId = request.EntityId,
            IsMain = request.SetAsMain,
            Alt = request.Alt,
            
            // URLs for different sizes
            OriginalUrl = uploadResult.PublicUrl,
            ThumbnailUrl = thumbnailResult.PublicUrl,
            MediumUrl = mediumResult.PublicUrl,
            LargeUrl = largeResult.PublicUrl,
            
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }
}