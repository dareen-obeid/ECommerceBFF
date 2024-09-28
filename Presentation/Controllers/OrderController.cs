using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceBFF.DTOs;
using System.Threading.Tasks;
using System.Net;
using System;
using System.ComponentModel.DataAnnotations;

[Authorize]
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
    public async Task<IActionResult> GetOrderById(int id)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var order = await _orderServiceClient.GetOrderByIdAsync(id, token);
            return Ok(order);
        }
        catch (HttpRequestException ex)
        {
            // Return the exact error message from the microservice
            return StatusCode((int)HttpStatusCode.BadRequest, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred." });
        }
    }
    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetOrdersByCustomer(int customerId)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var orders = await _orderServiceClient.GetOrdersByCustomerIdAsync(customerId, token);
            return Ok(orders);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return StatusCode((int)HttpStatusCode.UnprocessableEntity, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _orderServiceClient.CreateOrderAsync(orderDto, token);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderDto.OrderId }, orderDto);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return StatusCode((int)HttpStatusCode.UnprocessableEntity, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _orderServiceClient.UpdateOrderAsync(id, orderDto, token);
            return NoContent();
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return StatusCode((int)HttpStatusCode.UnprocessableEntity, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred." });
        }
    }

    [HttpDelete("cancel/{id}")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _orderServiceClient.CancelOrderAsync(id, token);
            return NoContent();
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return StatusCode((int)HttpStatusCode.UnprocessableEntity, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred." });
        }
    }
}
