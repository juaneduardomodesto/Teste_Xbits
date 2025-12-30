using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.EntitiesValidation;
using Teste_Xbits.Domain.Interface;

namespace Teste_Xbits.API.IoC.Containers;

public static class ValidationContainer
{
    public static IServiceCollection AddValidationContainer(this IServiceCollection services)
    {
        services.AddScoped<IValidate<Login>, LoginValidation>();
        services.AddScoped<IValidate<User>, UserValidation>();
        services.AddScoped<IValidate<Token>, TokenValidation>();
        services.AddScoped<IValidate<ProductCategory>, ProductCategoryValidation>();
        services.AddScoped<IValidate<Product>, ProductValidation>();
        services.AddScoped<IValidate<ImageFiles>, ImageValidation>();
        return services;
    }
}