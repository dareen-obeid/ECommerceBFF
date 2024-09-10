using System.Threading.Tasks;
using ECommerceBFF.DTOs;
using ECommerceBFF.ServiceClient;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBFF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderServiceClient _orderServiceClient;

        public OrderController(OrderServiceClient orderServiceClient)
        {
            _orderServiceClient = orderServiceClient;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var order = await _orderServiceClient.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomer(int customerId)
        {
            var orders = await _orderServiceClient.GetOrdersByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            await _orderServiceClient.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrderDetails), new { id = orderDto.OrderId }, orderDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            await _orderServiceClient.UpdateOrderAsync(id, orderDto);
            return NoContent();
        }

        [HttpDelete("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            await _orderServiceClient.CancelOrderAsync(id);
            return NoContent();
        }
    }
}
