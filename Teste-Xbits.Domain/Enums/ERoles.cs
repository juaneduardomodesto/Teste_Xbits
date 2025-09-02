using System.ComponentModel;

namespace Teste_Xbits.Domain.Enums;

public enum ERoles : uint
{
    [Description("Administrador")]
    Administrator = 1,
    
    [Description("Empregado")]
    Employee = 2,
}