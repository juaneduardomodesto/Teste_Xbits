using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Handlers.NotificationHandler;

namespace Teste_Xbits.Api.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class LoginController(ILoginQueryService loginQueryService) : Controller
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IEnumerable<DomainNotification>))]
    public Task<LoginResponse?> Login([FromBody] LoginRequest registerRequest) =>
        loginQueryService.Login(registerRequest);
    
    
}