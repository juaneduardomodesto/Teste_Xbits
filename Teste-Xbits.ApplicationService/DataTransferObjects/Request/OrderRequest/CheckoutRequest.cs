using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.OrderRequest;

public record CheckoutRequest
{
    public required EPaymentMethod PaymentMethod { get; init; }
    public decimal ShippingCost { get; init; }
    public decimal Discount { get; init; }
    public string? Notes { get; init; }
}