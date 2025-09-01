using FluentValidation;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;

namespace Teste_Xbits.Domain.EntitiesValidation;

public class ProductValidation : Validate<Product>
{
    public ProductValidation()
    {
        SetRule();
    }

    void SetRule()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(200)
            .WithMessage("O nome do produto não pode exceder 200 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("A descrição do produto não pode exceder 1000 caracteres.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("O preço do produto deve ser maior que zero.")
            .LessThan(1000000)
            .WithMessage("O preço do produto não pode exceder R$ 1.000.000,00.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("O código do produto é obrigatório.")
            .MaximumLength(50)
            .WithMessage("O código do produto não pode exceder 50 caracteres.");

        RuleFor(x => x.ExpirationDate)
            .Must((product, expirationDate) => ValidateExpirationDate(product, expirationDate))
            .WithMessage("Data de validade é obrigatória quando o produto tem data de validade.");

        RuleFor(x => x.ProductCategoryId)
            .GreaterThan(0)
            .When(x => x.ProductCategoryId.HasValue)
            .WithMessage("O ID da categoria de produto deve ser maior que zero.");
    }

    private bool ValidateExpirationDate(Product product, DateTime? expirationDate)
    {
        if (product.HasExpirationDate)
        {
            return expirationDate.HasValue;
        }
        
        return true;
    }
}