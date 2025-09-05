using FluentValidation;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;

namespace Teste_Xbits.Domain.EntitiesValidation;

public class ProductCategoryValidation : Validate<ProductCategory>
{
    public ProductCategoryValidation()
    {
        SetRule();
    }

    void SetRule()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome da categoria é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome da categoria não pode exceder 100 caracteres.")
            .MinimumLength(2)
            .WithMessage("O nome da categoria deve ter pelo menos 2 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("A descrição da categoria é obrigatória.")
            .MaximumLength(500)
            .WithMessage("A descrição não pode exceder 500 caracteres.")
            .MinimumLength(10)
            .WithMessage("A descrição deve ter pelo menos 10 caracteres.");

        RuleFor(x => x.ProductCategoryCode)
            .NotEmpty()
            .WithMessage("O código da categoria é obrigatório.")
            .MaximumLength(20)
            .WithMessage("O código da categoria não pode exceder 20 caracteres.");
    }
}