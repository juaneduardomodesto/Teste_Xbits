using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Infra.Interfaces.ServiceContracts;
using Teste_Xbits.Infra.Services;

namespace Teste_Xbits.API.IoC.Containers;

public static class PaginationContainer
{
    public static IServiceCollection AddPaginationContainer(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPaginationQueryService<>), typeof(PaginationQueryService<>));
        services.AddScoped<IPaginationQueryService<User>, PaginationQueryService<User>>();
        services.AddScoped<IPaginationQueryService<ProductCategory>, PaginationQueryService<ProductCategory>>();
        services.AddScoped<IPaginationQueryService<Product>, PaginationQueryService<Product>>();
        return services;
    }
}