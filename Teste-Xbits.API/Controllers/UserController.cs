using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Handlers.NotificationHandler;

namespace Teste_Xbits.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserCommandService userCommandService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("user_register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> FirstRegisterUser([FromBody] UserRegisterRequest registerRequest) =>
        userCommandService.RegisterUser(registerRequest);
}