using FluentValidation;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;
using Teste_Xbits.Domain.Handlers.ValidationHandler;

namespace Teste_Xbits.Domain.EntitiesValidation;

public sealed class LoginValidation : Validate<Login>
{
    public LoginValidation()
    {
        SetRules();
    }

    private void SetRules()
    {
        RuleFor(l => l.UserName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage(l => string.IsNullOrWhiteSpace(l.UserName)
                ? EMessage.Required.GetDescription().FormatTo("Usuário")
                : EMessage.MoreExpected.GetDescription().FormatTo("Usuário", "no máximo {MaxLength} caracteres"));

        RuleFor(l => l.Password)
            .NotNull()
            .NotEmpty()
            .MaximumLength(20)
            .WithMessage(l => string.IsNullOrWhiteSpace(l.Password)
                ? EMessage.Required.GetDescription().FormatTo("Senha")
                : EMessage.MoreExpected.GetDescription().FormatTo("Senha", "no máximo {MaxLength} caracteres"));
    }
}