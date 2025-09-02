using Teste_Xbits.API.Settings.Handlers;

namespace Teste_Xbits.API.Settings;

public static class SettingsControl
{
    public static void AddSettingsControl(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddProviderSettings(configuration);
        services.AddControllersSettings();
        services.AddCorsSettings(configuration);
        services.AddDatabaseConnectionSettings();
        services.AddAuthenticationSettings(configuration);
        services.AddAuthorizationSettings();
        services.AddFiltersSettings();
        services.AddSwaggerSettings();
        services.AddRateLimitingSettings();
    }
}