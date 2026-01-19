using FluentValidation;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;

namespace Teste_Xbits.Domain.EntitiesValidation;

public class OrderItemValidation : Validate<OrderItem>
{
    private const string FieldOrderId = "ID do pedido";
    private const string FieldProductId = "ID do produto";
    private const string FieldProductName = "Nome do produto";
    private const string FieldQuantity = "Quantidade";
    private const string FieldUnitPrice = "Preço unitário";
    private const string FieldCreationDate = "Data de criação";
    private const string FieldUpdateDate = "Data de atualização";
    private const string MsgProductNameTooLong = "Nome do produto muito longo";
    private const string MsgUpdateBeforeCreation = "Data de atualização anterior à criação";
    private const string MsgFutureDate = "Data futura";

    public OrderItemValidation()
    {
        SetRule();
    }

    void SetRule()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0).WithMessage(EMessage.InvalidId.GetDescription()
                .FormatTo(FieldOrderId));

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage(EMessage.InvalidId.GetDescription()
                .FormatTo(FieldProductId));

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage(EMessage.Required.GetDescription()
                .FormatTo(FieldProductName))
            .MaximumLength(255).WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(MsgProductNameTooLong));

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(EMessage.MoreExpected.GetDescription()
                .FormatTo(FieldQuantity, "maior que zero"));

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage(EMessage.MoreExpected.GetDescription()
                .FormatTo(FieldUnitPrice, "maior que zero"));
    }
}