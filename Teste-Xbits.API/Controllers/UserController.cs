using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste_Xbits.API.Extensions;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.UserResponse;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Handlers.NotificationHandler;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService) 
    : ControllerBase
{
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("register_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> RegisterUser([FromBody] UserRegisterRequest dtoRegister) =>
        userCommandService.RegisterUserAsync(dtoRegister, User.GetUserId(), false);
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("update_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> UpdateUser([FromBody] UserUpdateRequest dtoUpdate) =>
        userCommandService.UpdateUserAsync(dtoUpdate, User.GetUserCredential());
    
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("delete_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> DeleteUser([FromBody] UserDeleteRequest dtoDelete) =>
        userCommandService.DeleteUserAsync(dtoDelete, User.GetUserCredential());

    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("get_by_id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<UserResponse?> FindById([FromQuery] long userId) =>
        userQueryService.FindByIdAsync(userId);
    
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("list_users_paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<PageList<UserResponse>> GetUsersPaginatedAsync(
        [FromQuery] string? namePrefix,
        [FromQuery] string? emailPrefix,
        [FromQuery] string? cpfPrefix,
        [FromQuery] PageParams pageParams) =>
        userQueryService.FindAllWithPaginationAsync(namePrefix, emailPrefix, cpfPrefix, pageParams);
}