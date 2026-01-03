namespace Teste_Xbits.Domain.Enums;

public enum EOrderStatus
{
    Pending = 1,        // Aguardando pagamento
    Processing = 2,
    Confirmed = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6,
    Refunded = 7
}