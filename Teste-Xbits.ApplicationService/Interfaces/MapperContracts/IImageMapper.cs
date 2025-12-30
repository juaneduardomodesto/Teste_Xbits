using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.Interfaces.ServiceContracts;

namespace Teste_Xbits.ApplicationService.Interfaces.MapperContracts;

public interface IImageMapper
{
    ImageResponse DomainToResponse(ImageFiles image);
    IEnumerable<ImageResponse> DomainListToResponseList(IEnumerable<ImageFiles> images);
    
    /// <summary>
    /// Converts a multiple upload request into individual upload requests
    /// </summary>
    /// <param name="multipleRequest">The multiple upload request containing multiple files</param>
    /// <returns>A collection of individual upload requests</returns>
    /// <remarks>
    /// This method splits a multi-file upload into separate requests for individual processing.
    /// </remarks>
    IEnumerable<ImageRegisterRequest> MultipleToIndividualRequests(ImageMultipleUploadRequest multipleRequest);
    
    /// <summary>
    /// Converts an upload request to an ImageFiles domain entity
    /// </summary>
    /// <param name="request">The image registration request</param>
    /// <param name="uploadResult">Result of the original image upload</param>
    /// <param name="thumbnailResult">Result of the thumbnail upload</param>
    /// <param name="mediumResult">Result of the medium size upload</param>
    /// <param name="largeResult">Result of the large size upload</param>
    /// <returns>An ImageFiles domain entity</returns>
    ImageFiles UploadRequestToDomain(
        ImageRegisterRequest request,
        ImageUploadResult uploadResult,
        ImageUploadResult thumbnailResult,
        ImageUploadResult mediumResult,
        ImageUploadResult largeResult);
}