using System.ComponentModel;

namespace Teste_Xbits.Domain.Enums.ValidationEnum;

public enum EMessage : ushort
{
    [Description("{0} obrigatório.")]
    Required,
    
    [Description("{0} deve ter {1}.")]
    MoreExpected,
    
    [Description("Senhas não são iguais.")]
    PasswordNotMatch,
    
    [Description("{0} já possui um registro no sistema.")]
    Exist,
    
    [Description("Usuario não encontrado.")]
    UserNotFound,
    
    [Description("Não encontrado.")]
    NotFound,
    
    [Description("{0} não encontrado.")]
    ItemNotFound,
    
    [Description("Usuario inativo no sistema.")]
    InactiveUser,
    
    [Description("E-mail ou Senha incorreto.")]
    InvalidCredentials,
    
    [Description("Erro ao gerar token.")]
    TokenError,
    
    [Description("Id invalido.")]
    InvalidId,
    
    [Description("{0} em formato incorreto.")]
    InvalidValue,
    
    [Description("Valor monetario invalido.")]
    InvalidMonetaryValue,
    
    [Description("Produto não encontrado.")]
    ProductNotFound,
    
    [Description("Categoria não encontrada.")]
    CategoryNotFound,
    
    [Description("{0} não pertence ao usuário.")]
    NotOwnedByUser,

    [Description("{0} indisponível.")]
    Unavailable,

    [Description("{0} esgotado.")]
    OutOfStock,

    [Description("Quantidade máxima de {0} excedida.")]
    MaxQuantityExceeded,

    [Description("{0} inválido para a operação atual.")]
    InvalidForOperation,

    [Description("Operação não permitida em {0}.")]
    OperationNotAllowed,

    [Description("Limite de {0} atingido.")]
    LimitReached,

    [Description("{0} já adicionado ao carrinho.")]
    AlreadyAdded,

    [Description("Carrinho vazio.")]
    EmptyCart,

    [Description("Valor mínimo de {0} não atingido.")]
    MinimumValueNotReached,

    [Description("Erro ao processar {0}.")]
    ProcessingError,

    [Description("{0} expirado.")]
    Expired,
}