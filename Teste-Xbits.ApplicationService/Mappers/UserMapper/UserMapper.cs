using Microsoft.Extensions.Configuration;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.UserRegister;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.UserResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.ApplicationService.Mappers.UserMapper;

public class UserMapper(IConfiguration configuration) : IUserMapper
{
    private readonly string? _applicationSalt = configuration["Security:PasswordSalt"];

    public User DtoRegisterToDomain(UserRegisterRequest dtoRegister) =>
        new()
        {
            Name = dtoRegister.Name,
            Email = dtoRegister.Email,
            Cpf = dtoRegister.Cpf,
            PasswordHash = dtoRegister.Password.ConvertMd5(_applicationSalt!),
            AcceptPrivacyPolicy = dtoRegister.AcceptPrivacyPolicy,
            AcceptTermsOfUse = dtoRegister.AcceptTermsOfUse,
            IsActive = dtoRegister.IsActive,
            Role = dtoRegister.Roles,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

    public User DtoUpdateToDomain(UserUpdateRequest dtoUpdate, long userId) =>
        new()
        {
            Id = userId,
            Name = dtoUpdate.Name,
            Email = dtoUpdate.Email,
            Cpf = dtoUpdate.Cpf,
            PasswordHash = dtoUpdate.Password.ConvertMd5(_applicationSalt!),
            AcceptPrivacyPolicy = dtoUpdate.AcceptPrivacyPolicy,
            AcceptTermsOfUse = dtoUpdate.AcceptTermsOfUse,
            IsActive = dtoUpdate.IsActive,
            Role = dtoUpdate.Roles,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };

    public UserResponse DomainToSimpleResponse(User user)
    {
        return new UserResponse()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Cpf = user.Cpf,
            AcceptPrivacyPolicy = user.AcceptPrivacyPolicy,
            AcceptTermsOfUse = user.AcceptTermsOfUse,
            IsActive = user.IsActive,
            Role = user.Role,
        };
    }

    public PageList<UserResponse> DomainToPaginationUserResponse(PageList<User> userPageList)
    {
        var responses = userPageList.Items.Select(DomainToSimpleResponse).ToList();

        return new PageList<UserResponse>(
            responses,
            userPageList.TotalCount,
            userPageList.CurrentPage,
            userPageList.PageSize);
    }
}