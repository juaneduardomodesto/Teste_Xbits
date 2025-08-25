using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;
using Teste_Xbits.Infra.ORM.Context;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Teste_Xbits.ApplicationService.Services.TokenService;

public class TokenCommandCommandService : ServiceBase<Token>, ITokenCommandService
{
    private readonly ApplicationContext _context;
    private readonly IValidate<Token> _validate;
    private readonly ILoggerHandler _loggerHandler;
    private readonly IConfiguration _configuration;
    private readonly INotificationHandler _notification;
    private readonly ITokenMapper _tokenMapper;
    private readonly ITokenRepository _tokenRepository;

    public TokenCommandCommandService(
        ApplicationContext dbContext,
        IConfiguration configuration,
        INotificationHandler notification,
        ITokenMapper tokenMapper,
        IValidate<Token> validate,
        ILoggerHandler logger,
        ITokenRepository tokenRepository)
        : base(notification, validate, logger)
    {
        this._configuration = configuration;
        this._context = dbContext;
        this._notification = notification;
        this._tokenMapper = tokenMapper;
        this._tokenRepository = tokenRepository;
    }

    public async Task<TokenResponse?> Authentication(LoginRequest dtoLogin)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = _configuration["Jwt:JwtKey"]; // Corrigido para "JwtKey"
        var durationInMinutes = int.Parse(_configuration["Jwt:DurationInMinutes"]);
        var tokenExpirityTimeStamp = DateTime.UtcNow.AddMinutes(durationInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, dtoLogin.Email),
                new Claim(JwtRegisteredClaimNames.Email, dtoLogin.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = tokenExpirityTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), // Usando UTF8 instead of ASCII
                SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return _tokenMapper.MapToTokenResponse(
            accessToken,
            dtoLogin.Email,
            (int)tokenExpirityTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds);
    }
}