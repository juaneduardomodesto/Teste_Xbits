using System.Text.RegularExpressions;

namespace Teste_Xbits.Domain.Extensions;

public static partial class EmailExtension
{
    [GeneratedRegex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
    private static partial Regex EmailWithRegex();
    
    public static bool ValidateEmail(this string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        var regex = EmailWithRegex();
        var match = regex.Match(email);
        return match.Success;
    }
}