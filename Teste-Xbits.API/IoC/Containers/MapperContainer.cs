using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Mappers.CartMapper;
using Teste_Xbits.ApplicationService.Mappers.ImageMapper;
using Teste_Xbits.ApplicationService.Mappers.LoginMapper;
using Teste_Xbits.ApplicationService.Mappers.OrderMapper;
using Teste_Xbits.ApplicationService.Mappers.ProductCategoryMapper;
using Teste_Xbits.ApplicationService.Mappers.ProductMapper;
using Teste_Xbits.ApplicationService.Mappers.TokenMapper;
using Teste_Xbits.ApplicationService.Mappers.UserMapper;

namespace Teste_Xbits.API.IoC.Containers;

public static class MapperContainer
{
    public static IServiceCollection AddMapperContainer(this IServiceCollection services)
    {
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<ITokenMapper, TokenMapper>();
        services.AddScoped<ILoginMapper, LoginMapper>();
        services.AddScoped<IProductCategoryMapper, ProductCategoryMapper>();
        services.AddScoped<IProductMapper, ProductMapper>();
        services.AddScoped<IImageMapper, ImageMapper>();
        services.AddScoped<ICartMapper, CartMapper>();
        services.AddScoped<IOrderMapper, OrderMapper>();
        return services;
    }
}