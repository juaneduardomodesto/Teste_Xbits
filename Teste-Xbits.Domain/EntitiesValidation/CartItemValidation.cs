using FluentValidation;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;

namespace Teste_Xbits.Domain.EntitiesValidation;

public class CartItemValidation : Validate<CartItem>
{
    private const string FieldCartId = "ID do carrinho";
    private const string FieldProductId = "ID do produto";
    private const string FieldQuantity = "Quantidade";
    private const string FieldUnitPrice = "Preço unitário";
    private const string FieldCreationDate = "Data de criação";
    private const string FieldUpdateDate = "Data de atualização";
    private const string MsgFutureCreationDate = "Data de criação futura";
    private const string MsgUpdateBeforeCreation = "Data de atualização anterior à criação";
    private const string MsgFutureUpdateDate = "Data de atualização futura";
    private const string MsgUnitPriceTooHigh = "Preço unitário muito alto";

    public CartItemValidation()
    {
        SetRule();
    }

    void SetRule()
    {
        RuleFor(x => x.CartId)
            .NotEmpty().WithMessage(EMessage.Required.GetDescription()
                .FormatTo(FieldCartId))
            .GreaterThan(0).WithMessage(EMessage.InvalidId.GetDescription()
                .FormatTo(FieldCartId));

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage(EMessage.Required.GetDescription()
                .FormatTo(FieldProductId))
            .GreaterThan(0).WithMessage(EMessage.InvalidId.GetDescription()
                .FormatTo(FieldProductId));

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(EMessage.MoreExpected.GetDescription()
                .FormatTo(FieldQuantity, "maior que zero"))
            .LessThanOrEqualTo(1000).WithMessage(EMessage.MaxQuantityExceeded.GetDescription()
                .FormatTo("1000 unidades"));

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage(EMessage.MoreExpected.GetDescription()
                .FormatTo(FieldUnitPrice, "maior que zero"))
            .LessThanOrEqualTo(9999999.99m).WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(MsgUnitPriceTooHigh));

        RuleFor(x => x.CreatedAt)
            .NotEmpty().WithMessage(EMessage.Required.GetDescription()
                .FormatTo(FieldCreationDate))
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(MsgFutureCreationDate));

        RuleFor(x => x.UpdatedAt)
            .NotEmpty().WithMessage(EMessage.Required.GetDescription()
                .FormatTo(FieldUpdateDate))
            .GreaterThanOrEqualTo(x => x.CreatedAt)
                .When(x => x.CreatedAt != default && x.UpdatedAt != default)
                .WithMessage(EMessage.InvalidValue.GetDescription()
                    .FormatTo(MsgUpdateBeforeCreation))
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(MsgFutureUpdateDate));
    }
}