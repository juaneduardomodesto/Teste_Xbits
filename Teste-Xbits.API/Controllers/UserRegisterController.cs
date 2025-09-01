using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Handlers.NotificationHandler;

namespace Teste_Xbits.Api.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class UserRegisterController(IUserCommandFacadeService userCommandFacadeService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<bool> RegisterUser([FromBody] UserRegisterRequest dtoRegister) =>
        userCommandFacadeService.RegisterUserAsync(dtoRegister, Guid.Empty, true);
}