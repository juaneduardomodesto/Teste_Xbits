using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.LoginRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.TokenResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Interface;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Teste_Xbits.ApplicationService.Services.TokenService;

public class TokenCommandCommandService(
    IConfiguration configuration,
    INotificationHandler notification,
    ITokenMapper tokenMapper,
    IValidate<Token> validate,
    ILoggerHandler logger)
    : ServiceBase<Token>(notification, validate, logger), ITokenCommandService
{

    public async Task<TokenResponse?> Authentication(LoginRequest dtoLogin, Guid userGuid )
    {
        await Task.CompletedTask;
        
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        var key = configuration["Jwt:JwtKey"];
        var durationInMinutes = int.Parse(configuration["Jwt:DurationInMinutes"]!);
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(durationInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, userGuid.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, dtoLogin.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ]),
            Expires = tokenExpiryTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return tokenMapper.MapToTokenResponse( 
            accessToken,
            dtoLogin.Email!,
            (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds);
    }
}