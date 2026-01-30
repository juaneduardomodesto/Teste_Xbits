using FluentValidation;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;

namespace Teste_Xbits.Domain.EntitiesValidation;

public sealed class OrderValidation : Validate<Order>
{
    private const string FieldOrderNumber = "Número do pedido";
    private const string FieldUserId = "ID do usuário";
    private const string FieldCartId = "ID do carrinho";
    private const string FieldOrderStatus = "Status do pedido";
    private const string FieldPaymentMethod = "Método de pagamento";
    private const string FieldPaymentStatus = "Status do pagamento";
    private const string FieldSubtotal = "Subtotal";
    private const string FieldDiscount = "Desconto";
    private const string FieldShippingCost = "Custo de envio";
    private const string FieldTotal = "Total";
    private const string FieldCreationDate = "Data de criação";
    private const string FieldUpdateDate = "Data de atualização";
    private const string MsgOrderNumberTooLong = "Número do pedido muito longo";
    private const string MsgNegativeValue = "Valor negativo";
    private const string MsgDecimalPrecision = "Formato decimal incorreto";
    private const string MsgTotalMustBePositive = "Total deve ser maior que zero";
    private const string MsgCancellationReasonRequired = "Motivo do cancelamento obrigatório";
    private const string MsgCancellationReasonTooLong = "Motivo do cancelamento muito longo";
    private const string MsgFutureDate = "Data futura";
    private const string MsgUpdateBeforeCreation = "Data de atualização anterior à criação";

    public OrderValidation()
    {
        SetRules();
    }

    void SetRules()
    {
        RuleFor(order => order.OrderNumber)
            .NotEmpty()
            .WithMessage(EMessage.Required.GetDescription()
                .FormatTo(FieldOrderNumber))
            .MaximumLength(50)
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(MsgOrderNumberTooLong));

        RuleFor(order => order.UserId)
            .GreaterThan(0)
            .WithMessage(EMessage.Required.GetDescription()
                .FormatTo($"{FieldUserId} deve ser maior que zero"));

        RuleFor(order => order.CartId)
            .GreaterThan(0)
            .WithMessage(EMessage.InvalidId.GetDescription()
                .FormatTo(FieldCartId));

        RuleFor(order => order.Status)
            .IsInEnum()
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(FieldOrderStatus));

        RuleFor(order => order.PaymentMethod)
            .IsInEnum()
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(FieldPaymentMethod));

        RuleFor(order => order.PaymentStatus)
            .IsInEnum()
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(FieldPaymentStatus));

        RuleFor(order => order.Subtotal)
            .GreaterThanOrEqualTo(0)
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo($"{FieldSubtotal} {MsgNegativeValue}"))
            .ScalePrecision(2, 18)
            .WithMessage(EMessage.InvalidMonetaryValue.GetDescription()
                .FormatTo(MsgDecimalPrecision));

        RuleFor(order => order.Discount)
            .GreaterThanOrEqualTo(0)
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo($"{FieldDiscount} {MsgNegativeValue}"))
            .ScalePrecision(2, 18)
            .WithMessage(EMessage.InvalidMonetaryValue.GetDescription()
                .FormatTo(MsgDecimalPrecision));

        RuleFor(order => order.ShippingCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo($"{FieldShippingCost} {MsgNegativeValue}"))
            .ScalePrecision(2, 18)
            .WithMessage(EMessage.InvalidMonetaryValue.GetDescription()
                .FormatTo(MsgDecimalPrecision));

        RuleFor(order => order.Total)
            .GreaterThan(0)
            .WithMessage(EMessage.MoreExpected.GetDescription()
                .FormatTo(FieldTotal, MsgTotalMustBePositive))
            .ScalePrecision(2, 18)
            .WithMessage(EMessage.InvalidMonetaryValue.GetDescription()
                .FormatTo(MsgDecimalPrecision));

        RuleFor(order => order.CancellationReason)
            .MaximumLength(500)
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(MsgCancellationReasonTooLong))
            .NotEmpty()
            .WithMessage(EMessage.Required.GetDescription()
                .FormatTo(MsgCancellationReasonRequired))
            .When(order => order.Status == EOrderStatus.Cancelled);
    }
}