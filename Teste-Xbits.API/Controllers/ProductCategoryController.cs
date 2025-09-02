using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste_Xbits.API.Extensions;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductCategoryResponse;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Handlers.NotificationHandler;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoryController(
    IProductCategoryCommandService productCategoryCommandService,
    IProductCategoryQueryService categoryQueryService) : ControllerBase
{
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("register_product_category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> RegisterProductCategory([FromBody] ProductCategoryRegisterRequest dtoRegister) =>
        productCategoryCommandService.RegisterAsync(dtoRegister, User.GetUserCredential());
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("update_product_category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> UpdateProductCategory([FromBody] ProductCategoryUpdateRequest dtoUpdate) =>
        productCategoryCommandService.UpdateRegisterAsync(dtoUpdate, User.GetUserCredential());
    
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("delete_product_category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> DeleteProductCategory([FromBody] ProductCategoryDeleteRequest dtoDelete) =>
        productCategoryCommandService.DeleteRegisterAsync(dtoDelete, User.GetUserCredential());
    
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("get_by_id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<ProductCategoryResponse?> FindById([FromQuery] long productCategoryId) =>
        categoryQueryService.FindByIdAsync(productCategoryId);
    
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("list_product_category_paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<PageList<ProductCategoryResponse>> GetProductCategoryPaginatedAsync(       
            [FromQuery] string? namePrefix,
            [FromQuery] string? descriptionPrefix,
            [FromQuery] string? codePrefix,
            [FromQuery] PageParams pageParams) =>
            categoryQueryService.FindAllWithPaginationAsync(namePrefix, descriptionPrefix ,codePrefix, pageParams);
}