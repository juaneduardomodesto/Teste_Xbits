using Teste_Xbits.API.IoC;
using Teste_Xbits.API.Settings;
using Teste_Xbits.API.Settings.Handlers;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddSettingsControl(configuration);
builder.Services.AddInversionOfControlHandler();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.AddWebApplication(configuration);
await app.MigrateDatabaseAsync(configuration);
app.Run();

namespace Teste_Xbits.Api
{
    public abstract partial class Program
    {
    }
}