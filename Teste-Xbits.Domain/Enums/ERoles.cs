using System.ComponentModel;

namespace Teste_Xbits.Domain.Enums;

/// <summary>
/// User role enumeration
/// </summary>
public enum ERoles : uint
{
    [Description("Administrador")]
    Administrator = 1,
    [Description("Funcionário")]
    Employee = 2,
    [Description("Cliente")]
    Customer = 3,
}