using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste_Xbits.API.Extensions;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.CartRequest;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.CartResponse;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Domain.Handlers.NotificationHandler;

namespace Teste_Xbits.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController(
    ICartCommandService cartCommandService,
    ICartQueryService cartQueryService)
    : ControllerBase
{
    /// <summary>
    /// Retrieves the active cart for the authenticated user
    /// </summary>
    /// <returns>The user's active cart details or null if no active cart exists</returns>
    /// <remarks>
    /// This endpoint returns the current active shopping cart for the authenticated user.
    /// The cart includes all items, quantities, prices, and total calculations.
    /// If no active cart exists for the user, the system may return null or create a new empty cart.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpGet("list_cart_items_by_user")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<CartResponse?> GetMyCart() =>
        await cartQueryService.GetActiveCartAsync(User.GetUserId());

    /// <summary>
    /// Adds a product to the user's shopping cart
    /// </summary>
    /// <param name="request">The request containing product ID and quantity to add</param>
    /// <returns>The added cart item details or null if operation fails</returns>
    /// <remarks>
    /// This endpoint allows adding a product to the authenticated user's shopping cart.
    /// The system will:
    /// - Validate product availability
    /// - Check inventory levels
    /// - Create or update cart item
    /// - Calculate pricing and totals
    /// If the product already exists in the cart, the quantity may be incremented based on business rules.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpPost("add_product_to_cart")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CartItemResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<CartItemResponse?> AddToCart([FromBody] AddToCartRequest request) =>
        await cartCommandService.AddToCartAsync(request, User.GetUserCredential());

    /// <summary>
    /// Updates the quantity of a product in the user's cart
    /// </summary>
    /// <param name="request">The request containing cart item ID and new quantity</param>
    /// <returns>True if update was successful, false otherwise</returns>
    /// <remarks>
    /// This endpoint allows updating the quantity of an existing item in the cart.
    /// The system will:
    /// - Validate the cart item belongs to the user
    /// - Check new quantity against product availability
    /// - Update pricing calculations
    /// - Recalculate cart totals
    /// Setting quantity to 0 will typically remove the item from the cart.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpPut("update_product_from_user_cart")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<bool> UpdateCartItem([FromBody] UpdateCartItemRequest request) =>
        await cartCommandService.UpdateCartItemAsync(request, User.GetUserCredential());

    /// <summary>
    /// Removes a specific product from the user's cart
    /// </summary>
    /// <param name="request">The request containing the cart item ID to remove</param>
    /// <returns>True if removal was successful, false otherwise</returns>
    /// <remarks>
    /// This endpoint allows removing a specific item from the shopping cart.
    /// The system will:
    /// - Validate the cart item belongs to the user
    /// - Remove the item from the cart
    /// - Recalculate cart totals
    /// - Update inventory availability if applicable
    /// This operation is irreversible within the same session.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpDelete("remove_product_from_user_cart")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<bool> RemoveFromCart([FromBody] RemoveFromCartRequest request) =>
        await cartCommandService.RemoveFromCartAsync(request, User.GetUserCredential());

    /// <summary>
    /// Clears all items from the user's shopping cart
    /// </summary>
    /// <returns>True if the cart was cleared successfully, false otherwise</returns>
    /// <remarks>
    /// This endpoint removes all items from the authenticated user's shopping cart.
    /// The system will:
    /// - Empty the cart completely
    /// - Reset all cart calculations
    /// - Update inventory availability for all removed items
    /// - Maintain the cart record for future use
    /// This operation is typically used for cart abandonment or starting fresh.
    /// Use with caution as this operation cannot be undone.
    /// Requires <see cref="ERoles.Employee"/> or <see cref="ERoles.Administrator"/> role.
    /// </remarks>
    [Authorize(Policy = "EmployeeOrAdmin")]
    [HttpDelete("clear_user_cart")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<bool> ClearCart() =>
        await cartCommandService.ClearCartAsync(User.GetUserCredential());
}