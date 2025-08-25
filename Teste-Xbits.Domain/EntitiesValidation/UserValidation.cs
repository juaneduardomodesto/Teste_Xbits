using FluentValidation;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;

namespace Teste_Xbits.Domain.EntitiesValidation;

public sealed class UserValidation : Validate<User>
{
    public UserValidation()
    {
        SetRules();
    }

    void SetRules()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MinimumLength(3)
            .WithMessage("O nome deve ter pelo menos 3 caracteres.")
            .MaximumLength(100)
            .WithMessage("O nome não pode exceder 100 caracteres.");

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("O email é obrigatório.")
            .EmailAddress()
            .WithMessage("Formato de email inválido.")
            .MaximumLength(150)
            .WithMessage("O email não pode exceder 150 caracteres.");

        RuleFor(user => user.Cpf)
            .NotEmpty()
            .WithMessage("O CPF é obrigatório.")
            .Length(11)
            .WithMessage("CPF deve conter 11 dígitos.");

        RuleFor(user => user.PasswordHash)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.")
            .MinimumLength(6)
            .WithMessage("A senha deve ter pelo menos 6 caracteres.");

        RuleFor(user => user.AcceptPrivacyPolicy)
            .Equal(true)
            .WithMessage("É necessário aceitar a política de privacidade.");

        RuleFor(user => user.AcceptTermsOfUse)
            .Equal(true)
            .WithMessage("É necessário aceitar os termos de uso.");
    }
}