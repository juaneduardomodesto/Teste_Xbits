namespace Teste_Xbits.ApplicationService.DataTransferObjects.Request.OrderRequest;

public record ProcessPaymentRequest
{
    public required long OrderId { get; init; }
    public string? TransactionId { get; init; }
    public string? PaymentDetails { get; init; }
}