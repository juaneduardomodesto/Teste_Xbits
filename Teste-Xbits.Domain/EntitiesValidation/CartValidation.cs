using FluentValidation;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Enums.ValidationEnum;
using Teste_Xbits.Domain.Extensions;

namespace Teste_Xbits.Domain.EntitiesValidation;

public class CartValidation : Validate<Cart>
{
    private const string FieldUserId = "ID do usuário";
    private const string FieldCartStatus = "Status do carrinho";
    private const string FieldCreationDate = "Data de criação";
    private const string FieldUpdateDate = "Data de atualização";
    private const string MsgOldCheckoutDate = "Data de checkout muito antiga";
    private const string MsgFutureCheckoutDate = "Data de checkout futura";
    private const string MsgFutureCreationDate = "Data de criação futura";
    private const string MsgUpdateBeforeCreation = "Data de atualização anterior à criação";
    private const string MsgFutureUpdateDate = "Data de atualização futura";
    private const string MsgCheckoutNotFinalized = "Data de checkout para carrinho não finalizado";
    private const string MsgCheckoutRequired = "Data de checkout para carrinho finalizado";
    private const string MsgCheckoutActive = "Data de checkout para carrinho ativo";
    private const string MsgCheckoutAbandoned = "Data de checkout para carrinho abandonado";

    public CartValidation()
    {
        SetRule();
    }

    void SetRule()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(EMessage.Required.GetDescription()
                .FormatTo(FieldUserId));

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage(EMessage.Required.GetDescription()
                .FormatTo(FieldCartStatus))
            .IsInEnum().WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(FieldCartStatus));

        RuleFor(x => x.CheckedOutAt)
            .GreaterThanOrEqualTo(DateTime.UtcNow.AddYears(-1))
                .When(x => x.CheckedOutAt.HasValue)
                .WithMessage(EMessage.InvalidValue.GetDescription()
                    .FormatTo(MsgOldCheckoutDate))
            .LessThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.CheckedOutAt.HasValue)
                .WithMessage(EMessage.InvalidValue.GetDescription()
                    .FormatTo(MsgFutureCheckoutDate));

        RuleFor(x => x.CreatedAt)
            .NotEmpty().WithMessage(EMessage.Required.GetDescription()
                .FormatTo(FieldCreationDate))
            .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage(EMessage.InvalidValue.GetDescription()
                    .FormatTo(MsgFutureCreationDate));

        RuleFor(x => x.UpdatedAt)
            .NotEmpty().WithMessage(EMessage.Required.GetDescription()
                .FormatTo(FieldUpdateDate))
            .GreaterThanOrEqualTo(x => x.CreatedAt)
                .When(x => x.CreatedAt != default && x.UpdatedAt != default)
                .WithMessage(EMessage.InvalidValue.GetDescription()
                    .FormatTo(MsgUpdateBeforeCreation))
            .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage(EMessage.InvalidValue.GetDescription()
                    .FormatTo(MsgFutureUpdateDate));
        
        RuleFor(x => x)
            .Must(x => !x.CheckedOutAt.HasValue || x.Status == ECartStatus.CheckedOut)
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(MsgCheckoutNotFinalized))
            .Must(x => x.Status != ECartStatus.CheckedOut || x.CheckedOutAt.HasValue)
            .WithMessage(EMessage.Required.GetDescription()
                .FormatTo(MsgCheckoutRequired))
            .Must(x => x.CheckedOutAt == null || x.Status != ECartStatus.Active)
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(MsgCheckoutActive))
            .Must(x => x.CheckedOutAt == null || x.Status != ECartStatus.Abandoned)
            .WithMessage(EMessage.InvalidValue.GetDescription()
                .FormatTo(MsgCheckoutAbandoned));
    }
}