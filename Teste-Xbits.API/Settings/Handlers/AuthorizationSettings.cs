

namespace Teste_Xbits.API.Settings.Handlers;

public static class AuthorizationSettings
{
    public static void AddAuthorizationSettings(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy => 
                policy.RequireRole("Administrator"))
            .AddPolicy("EmployeeOnly", policy => 
                policy.RequireRole("Employee"))
            .AddPolicy("EmployeeOrAdmin", policy => 
                policy.RequireRole("Employee", "Administrator"));
    }
}