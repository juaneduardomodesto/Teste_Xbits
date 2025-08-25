using Microsoft.Extensions.Configuration;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Extensions;

namespace Teste_Xbits.ApplicationService.Mappers.UserRegisterMapper;

public class UserMapper : IUserMapper
{
    private readonly string _applicationSalt;
    
    public UserMapper(IConfiguration configuration)
    {
        _applicationSalt = configuration["Security:PasswordSalt"];
    }

    public User DtoRegisterToDomain(UserRegisterRequest dtoRegister) =>
        new()
        {
            Name = dtoRegister.Name,
            Email = dtoRegister.Email,
            Cpf = dtoRegister.Cpf,
            PasswordHash = dtoRegister.Password.ConvertMd5(_applicationSalt),
            AcceptPrivacyPolicy = dtoRegister.AcceptPrivacyPolicy,
            AcceptTermsOfUse = dtoRegister.AcceptTermsOfUse,
            IsActive = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
}