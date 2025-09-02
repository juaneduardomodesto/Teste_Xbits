using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste_Xbits.API.Extensions;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductResponse;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.NotificationHandler;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ProductController(
    IProductCommandService productCommandService, 
    IProductQueryService productQueryService) 
    : ControllerBase
{
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("register_product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> RegisterProduct([FromBody] ProductRegisterRequest dtoRegister) =>
        productCommandService.RegisterProductAsync(dtoRegister, User.GetUserCredential());
    
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("update_product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> UpdateProduct([FromBody] ProductUpdateRequest dtoUpdate) =>
        productCommandService.UpdateProductAsync(dtoUpdate, User.GetUserCredential());
    
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("delete_product")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> DeleteProduct([FromBody] ProductDeleteRequest dtoDelete) =>
        productCommandService.DeleteProductAsync(dtoDelete, User.GetUserCredential());

    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("get_by_id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<ProductResponse?> FindById([FromQuery] long productId) =>
        productQueryService.FindByIdAsync(productId);

    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("list_products_paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<PageList<ProductResponse>> GetProductsPaginated(
        [FromQuery] string? namePrefix,
        [FromQuery] string? descriptionPrefix,
        [FromQuery] decimal? pricePrefix,
        [FromQuery] string? productCodePrefix,
        [FromQuery] bool? hasValidadeDatePrefix,
        [FromQuery] string? expirationDate,
        [FromQuery] long? productCategoryIdPrefix,
        [FromQuery] PageParams pageParams) =>
        productQueryService.FindAllWithPaginationAsync(
            namePrefix,
            descriptionPrefix,
            pricePrefix,
            productCodePrefix,
            hasValidadeDatePrefix,
            expirationDate,
            productCategoryIdPrefix,
            pageParams);
}