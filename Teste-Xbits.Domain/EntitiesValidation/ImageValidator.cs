using FluentValidation;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.Domain.EntitiesValidation;

public class ImageValidation : Validate<ImageFiles>
{
    public ImageValidation()
    {
        SetRule();
    }

    void SetRule()
    {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("Nome do arquivo é obrigatório")
            .MaximumLength(255).WithMessage("Nome do arquivo muito longo");

        RuleFor(x => x.StoragePath)
            .NotEmpty().WithMessage("Caminho de armazenamento é obrigatório")
            .MaximumLength(500).WithMessage("Caminho muito longo");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("Tipo de conteúdo é obrigatório")
            .Must(BeValidContentType).WithMessage("Tipo de conteúdo inválido");

        RuleFor(x => x.SizeInBytes)
            .GreaterThan(0).WithMessage("Tamanho do arquivo deve ser maior que zero")
            .LessThanOrEqualTo(5 * 1024 * 1024).WithMessage("Arquivo muito grande (máx. 5MB)");

        RuleFor(x => x.EntityId)
            .GreaterThan(0).WithMessage("ID da entidade inválido");
    }

    private bool BeValidContentType(string contentType)
    {
        var validTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/jpg" };
        return validTypes.Contains(contentType.ToLower());
    }
}