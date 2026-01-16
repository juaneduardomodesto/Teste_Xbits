using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste_Xbits.API.Extensions;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.OrderRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.OrderResponse;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Handlers.NotificationHandler;
using Teste_Xbits.Domain.Handlers.PaginationHandler;

namespace Teste_Xbits.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController(
    IOrderCommandService orderCommandService,
    IOrderQueryService orderQueryService)
    : ControllerBase
{
    /// <summary>
    /// Processes a checkout operation to create a new order from the user's cart
    /// </summary>
    /// <param name="request">The checkout request containing payment and shipping details</param>
    /// <returns>The created order details or null if operation fails</returns>
    /// <remarks>
    /// This endpoint finalizes the shopping cart and creates a new order.
    /// The system will:
    /// - Validate the cart contents and availability
    /// - Process payment information
    /// - Create order records
    /// - Generate order number
    /// - Mark cart as checked out
    /// - Update inventory levels
    /// The cart will be converted to a permanent order that cannot be modified.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpPost("checkout")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<OrderResponse?> Checkout([FromBody] CheckoutRequest request) =>
        await orderCommandService.CheckoutAsync(request, User.GetUserCredential());
        
    /// <summary>
    /// Processes payment for an existing order
    /// </summary>
    /// <param name="request">The payment processing request containing payment details</param>
    /// <returns>True if payment was processed successfully, false otherwise</returns>
    /// <remarks>
    /// This endpoint handles payment processing for an order.
    /// The system will:
    /// - Validate order status (must be pending payment)
    /// - Process payment through the configured payment gateway
    /// - Update order payment status
    /// - Generate payment confirmation
    /// - Send payment confirmation notifications
    /// This operation is typically triggered after checkout or for manual payment processing.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpPost("process-payment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<bool> ProcessPayment([FromBody] ProcessPaymentRequest request) =>
        await orderCommandService.ProcessPaymentAsync(request, User.GetUserCredential());

    /// <summary>
    /// Cancels an existing order with a specified reason
    /// </summary>
    /// <param name="orderId">The ID of the order to cancel</param>
    /// <param name="reason">The reason for cancellation</param>
    /// <returns>True if order was cancelled successfully, false otherwise</returns>
    /// <remarks>
    /// This endpoint cancels an order that hasn't been shipped or fully processed.
    /// The system will:
    /// - Validate order can be cancelled (based on status and business rules)
    /// - Update order status to cancelled
    /// - Record cancellation reason
    /// - Process refunds if applicable
    /// - Restore inventory levels
    /// - Send cancellation notifications
    /// Cancellation may not be possible for orders that have already been shipped or processed.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpPut("cancel/{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> CancelOrder(
        [FromRoute] long orderId,
        [FromBody] string reason) =>
        await orderCommandService.CancelOrderAsync(orderId, reason, User.GetUserCredential());

    /// <summary>
    /// Retrieves an order by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the order</param>
    /// <returns>The order details or null if not found</returns>
    /// <remarks>
    /// This endpoint retrieves detailed information about a specific order.
    /// The response includes:
    /// - Order header information (number, dates, status)
    /// - Order items with product details
    /// - Pricing breakdown (subtotal, taxes, shipping, total)
    /// - Payment and shipping information
    /// - User and cart references
    /// The authenticated user must have access rights to view the order.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("get_order_by_id/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<OrderResponse?> GetOrderById([FromRoute] long id) =>
        await orderQueryService.GetOrderByIdAsync(id);

    /// <summary>
    /// Retrieves an order by its order number
    /// </summary>
    /// <param name="orderNumber">The unique order number</param>
    /// <returns>The order details or null if not found</returns>
    /// <remarks>
    /// This endpoint retrieves detailed information about a specific order using its order number.
    /// The response includes:
    /// - Complete order details
    /// - All associated order items
    /// - Payment and shipping information
    /// - Status history
    /// Order numbers are typically human-readable identifiers used for customer reference.
    /// The authenticated user must have access rights to view the order.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("get_order_by_number/{orderNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<OrderResponse?> GetOrderByNumber([FromRoute] string orderNumber) =>
        await orderQueryService.GetOrderByNumberAsync(orderNumber);

    /// <summary>
    /// Retrieves a paginated list of orders for the authenticated user
    /// </summary>
    /// <param name="pageParams">Pagination parameters (page number, page size)</param>
    /// <returns>A paginated list of the user's orders</returns>
    /// <remarks>
    /// This endpoint retrieves the order history for the authenticated user.
    /// The system returns:
    /// - Paginated list of orders (most recent first by default)
    /// - Summary information for each order
    /// - Pagination metadata (total count, page info)
    /// Orders can be filtered by date range, status, or other criteria through query parameters.
    /// This is useful for order history pages and account management.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("my-orders")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PageList<OrderResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<PageList<OrderResponse>> GetMyOrders([FromQuery] PageParams pageParams) =>
        await orderQueryService.GetUserOrdersAsync(User.GetUserId(), pageParams);
}