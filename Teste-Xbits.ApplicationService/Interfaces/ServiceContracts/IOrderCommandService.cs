using Teste_Xbits.ApplicationService.DataTransferObjects.Request.OrderRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.OrderResponse;
using Teste_Xbits.Domain.Entities;

namespace Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;

public interface IOrderCommandService
{
    Task<OrderResponse?> CheckoutAsync(CheckoutRequest request, UserCredential userCredential);
    Task<bool> ProcessPaymentAsync(ProcessPaymentRequest request, UserCredential userCredential);
    Task<bool> CancelOrderAsync(long orderId, string reason, UserCredential userCredential);
}