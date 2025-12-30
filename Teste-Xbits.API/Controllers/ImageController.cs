using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste_Xbits.API.Extensions;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ImageRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ImageResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Handlers.NotificationHandler;

namespace Teste_Xbits.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageController(
    IImageCommandService imageCommandService,
    IImageQueryService imageQueryService,
    IImageMapper imageMapper)
    : ControllerBase
{
    /// <summary>
    /// Uploads a single image file
    /// </summary>
    /// <param name="request">The image registration request containing file and metadata</param>
    /// <returns>The uploaded image details or null if upload fails</returns>
    /// <remarks>
    /// This endpoint allows authenticated employees or admins to upload an image file.
    /// The image will be associated with a specific entity (e.g., product, user, etc.).
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpPost("upload_image")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImageResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ImageResponse?> UploadImage([FromForm] ImageRegisterRequest request) =>
        await imageCommandService.UploadImageAsync(request, User.GetUserCredential());

    /// <summary>
    /// Deletes a specific image by its ID
    /// </summary>
    /// <param name="request">The image deletion request containing the image ID</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    /// <remarks>
    /// This endpoint allows authenticated employees or admins to delete an existing image.
    /// The operation will remove the image file from storage and its record from the database.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpDelete("delete_image")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<bool> DeleteImage([FromBody] ImageDeleteRequest request) =>
        await imageCommandService.DeleteImageAsync(request, User.GetUserCredential());

    /// <summary>
    /// Sets an image as the main/default image for an entity
    /// </summary>
    /// <param name="request">The request containing the image ID to set as main</param>
    /// <returns>True if operation was successful, false otherwise</returns>
    /// <remarks>
    /// This endpoint allows setting a specific image as the primary/default image
    /// for an entity. Only one image can be the main image per entity.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpPut("set_main_image")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<bool> SetMainImage([FromBody] ImageSetMainRequest request) =>
        await imageCommandService.SetMainImageAsync(request, User.GetUserCredential());

    /// <summary>
    /// Retrieves a specific image by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the image</param>
    /// <returns>The image details or null if not found</returns>
    /// <remarks>
    /// This endpoint returns detailed information about a specific image,
    /// including metadata, file information, and entity association.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("get_by_id/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ImageResponse?> GetImageById([FromRoute] long id) =>
        await imageQueryService.FindByIdAsync(id);

    /// <summary>
    /// Retrieves all images associated with a specific entity
    /// </summary>
    /// <param name="entityType">The type of entity (e.g., <see cref="EEntityType.Product"/>, <see cref="EEntityType.User"/>)</param>
    /// <param name="entityId">The unique identifier of the entity</param>
    /// <returns>A collection of images associated with the entity</returns>
    /// <remarks>
    /// This endpoint returns all images that belong to a specific entity.
    /// The images are typically ordered by their display order.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("get_by_id/{entityType}/{entityId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ImageResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IEnumerable<ImageResponse>> GetImagesByEntity(
        [FromRoute] EEntityType entityType,
        [FromRoute] long entityId) =>
        await imageQueryService.FindByEntityAsync(entityType, entityId);

    /// <summary>
    /// Retrieves the main/default image for a specific entity
    /// </summary>
    /// <param name="entityType">The type of entity (e.g., <see cref="EEntityType.Product"/>, <see cref="EEntityType.User"/>)</param>
    /// <param name="entityId">The unique identifier of the entity</param>
    /// <returns>The main image details or null if no main image is set</returns>
    /// <remarks>
    /// This endpoint returns the primary/default image for an entity.
    /// If no main image is explicitly set, it might return the first image or null.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("get_main_image/{entityType}/{entityId}/main")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ImageResponse?> GetMainImage(
        [FromRoute] EEntityType entityType,
        [FromRoute] long entityId) =>
        await imageQueryService.GetMainImageAsync(entityType, entityId);

    /// <summary>
    /// Uploads multiple image files in a single request
    /// </summary>
    /// <param name="request">The multi-upload request containing files and metadata</param>
    /// <returns>A collection of uploaded image details</returns>
    /// <remarks>
    /// This endpoint allows uploading multiple image files at once.
    /// The first image can optionally be set as the main image for the entity.
    /// Each image can have its own alternative text (alt tag).
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpPost("upload-multiple")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<ImageResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ImageResponse>>> UploadMultipleImages(
        [FromForm] ImageMultipleUploadRequest request)
    {
        // Auxiliar mapping
        var individualRequests = imageMapper.MultipleToIndividualRequests(request);
        var results = new List<ImageResponse>();

        foreach (var uploadRequest in individualRequests)
        {
            var result = await imageCommandService.UploadImageAsync(uploadRequest, User.GetUserCredential());
            if (result != null)
            {
                results.Add(result);
            }
        }

        return results.Count != 0
            ? CreatedAtAction(nameof(GetImagesByEntity), 
                new { entityType = request.EntityType, entityId = request.EntityId }, results)
            : BadRequest("No images were successfully uploaded");
    }

    /// <summary>
    /// Updates the display order of images for an entity
    /// </summary>
    /// <param name="request">The request containing image IDs in the desired order</param>
    /// <returns>True if the order was updated successfully, false otherwise</returns>
    /// <remarks>
    /// This endpoint allows reordering images for an entity.
    /// The order determines how images are displayed in galleries or carousels.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpPut("update-order")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<bool> UpdateImageOrder([FromBody] ImageUpdateOrderRequest request) =>
        await imageCommandService.UpdateImageOrderAsync(request, User.GetUserCredential());

    /// <summary>
    /// Updates the alternative text (alt tag) of an image
    /// </summary>
    /// <param name="request">The request containing the image ID and new alt text</param>
    /// <returns>True if the alt text was updated successfully, false otherwise</returns>
    /// <remarks>
    /// This endpoint allows updating the accessibility description (alt text) of an image.
    /// Alt text is important for screen readers and SEO.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpPut("update-alt")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> UpdateImageAlt([FromBody] ImageUpdateAltRequest request) =>
        await imageCommandService.UpdateImageAltAsync(request, User.GetUserCredential());

    /// <summary>
    /// Deletes all images associated with a specific entity
    /// </summary>
    /// <param name="entityType">The type of entity (e.g., <see cref="EEntityType.Product"/>, <see cref="EEntityType.User"/>)</param>
    /// <param name="entityId">The unique identifier of the entity</param>
    /// <returns>True if all images were deleted successfully, false otherwise</returns>
    /// <remarks>
    /// This endpoint is restricted to <see cref="ERoles.Administrator"/> only.
    /// It permanently removes all images associated with the specified entity.
    /// Use with caution as this operation cannot be undone.
    /// </remarks>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("entity/{entityType}/{entityId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<bool> DeleteEntityImages(
        [FromRoute] EEntityType entityType,
        [FromRoute] long entityId) =>
        await imageCommandService.DeleteEntityImagesAsync(
            entityType, entityId, User.GetUserCredential());

    /// <summary>
    /// Downloads an image file directly
    /// </summary>
    /// <param name="id">The unique identifier of the image</param>
    /// <returns>The image file as a downloadable stream</returns>
    /// <remarks>
    /// This endpoint provides direct download access to image files.
    /// It's useful for systems that don't serve static files through a web server.
    /// The response includes appropriate content type and file name headers.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("download/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DownloadImage([FromRoute] long id)
    {
        var image = await imageQueryService.FindByIdAsync(id);
        if (image == null)
            return NotFound();

        var stream = await imageCommandService.DownloadImageAsync(id);
        if (stream == null)
            return NotFound();

        return File(stream, image.ContentType, image.FileName);
    }
}