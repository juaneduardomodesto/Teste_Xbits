using System.ComponentModel;

namespace Teste_Xbits.Domain.Enums;

/// <summary>
/// User role enumeration
/// </summary>
public enum ERoles : uint
{
    [Description("Administrador")]
    Administrator = 1,
    
    [Description("Empregado")]
    Client = 2,
}