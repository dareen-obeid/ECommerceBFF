using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceBFF.DTOs;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly CartServiceClient _cartServiceClient;

    public CartController(CartServiceClient cartServiceClient)
    {
        _cartServiceClient = cartServiceClient;
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetCartByCustomerId(int customerId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", ""); // Extract JWT token
        var cart = await _cartServiceClient.GetCartByCustomerIdAsync(customerId, token);
        return Ok(cart);
    }

    [HttpGet("items/{cartId}")]
    public async Task<IActionResult> GetCartItemsByCartId(int cartId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var items = await _cartServiceClient.GetCartItemsByCartIdAsync(cartId, token);
        return Ok(items);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddItemToCart([FromBody] CartItemDto cartItemDto)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await _cartServiceClient.AddItemToCartAsync(cartItemDto, token);
        return CreatedAtAction(nameof(GetCartItemsByCartId), new { cartId = cartItemDto.CartId }, cartItemDto);
    }

    [HttpPut("update/{cartItemId}")]
    public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] CartItemDto cartItemDto)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await _cartServiceClient.UpdateCartItemAsync(cartItemId, cartItemDto, token);
        return NoContent();
    }

    [HttpDelete("removeCartItem/{cartItemId}")]
    public async Task<IActionResult> RemoveCartItem(int cartItemId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await _cartServiceClient.RemoveCartItemAsync(cartItemId, token);
        return NoContent();
    }
}
