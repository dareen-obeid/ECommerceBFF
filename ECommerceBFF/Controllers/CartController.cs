using System.Threading.Tasks;
using ECommerceBFF.DTOs;
using ECommerceBFF.ServiceClient;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBFF.Controllers
{
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
            var cart = await _cartServiceClient.GetCartByCustomerIdAsync(customerId);
            return Ok(cart);
        }

        [HttpGet("items/{cartId}")]
        public async Task<IActionResult> GetCartItemsByCartId(int cartId)
        {
            var items = await _cartServiceClient.GetCartItemsByCartIdAsync(cartId);
            return Ok(items);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemDto cartItemDto)
        {
            await _cartServiceClient.AddItemToCartAsync(cartItemDto);
            return CreatedAtAction(nameof(GetCartItemsByCartId), new { cartId = cartItemDto.CartId }, cartItemDto);
        }

        [HttpPut("update/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] CartItemDto cartItemDto)
        {
            await _cartServiceClient.UpdateCartItemAsync(cartItemId, cartItemDto);
            return NoContent();
        }

        [HttpDelete("removeCartItem/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            await _cartServiceClient.RemoveCartItemAsync(cartItemId);
            return NoContent();
        }

        [HttpDelete("removeCart/{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            await _cartServiceClient.DeleteCartAsync(cartId);
            return NoContent();
        }
    }
}
