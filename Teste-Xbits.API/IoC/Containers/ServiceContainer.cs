using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.ApplicationService.Services.CartService;
using Teste_Xbits.ApplicationService.Services.ImageService;
using Teste_Xbits.ApplicationService.Services.LoginService;
using Teste_Xbits.ApplicationService.Services.OrderService;
using Teste_Xbits.ApplicationService.Services.ProductCategoryService;
using Teste_Xbits.ApplicationService.Services.ProductService;
using Teste_Xbits.ApplicationService.Services.Storage;
using Teste_Xbits.ApplicationService.Services.TokenService;
using Teste_Xbits.ApplicationService.Services.UserService;
using Teste_Xbits.Infra.Interfaces.ServiceContracts;
using Teste_Xbits.Infra.Services;

namespace Teste_Xbits.API.IoC.Containers;

public static class ServiceContainer
{
    public static IServiceCollection AddServiceContainer(this IServiceCollection services)
    {
        services.AddScoped<ILoginQueryService, LoginQueryService>();
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<IUserCommandFacadeService, UserCommandService>();
        services.AddScoped<IUserQueryService,  UserQueryService>();
        services.AddScoped<ITokenCommandService, TokenCommandCommandService>();
        services.AddScoped<IProductCategoryCommandService, ProductCategoryCommandService>();
        services.AddScoped<IProductCategoryQueryService, ProductCategoryQueryService>();
        services.AddScoped<IProductCommandService, ProductCommandService>();
        services.AddScoped<IProductQueryService, ProductQueryService>();
        services.AddScoped<IImageCommandService, ImageCommandService>();
        services.AddScoped<IImageQueryService, ImageQueryService>();
        services.AddScoped<IImageStorageService, LocalImageStorageService>();
        services.AddScoped<IImageResizerService, ImageResizer>();
        services.AddScoped<ICartCommandService, CartCommandService>();
        services.AddScoped<ICartQueryService, CartQueryService>();
        services.AddScoped<IOrderCommandService, OrderCommandService>();
        services.AddScoped<IOrderQueryService, OrderQueryService>();
        return services;
    }
}