using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceBFF.DTOs;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductServiceClient _productServiceClient;

    public ProductController(ProductServiceClient productServiceClient)
    {
        _productServiceClient = productServiceClient;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductDetails(int id)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var product = await _productServiceClient.GetProductByIdAsync(id, token);
        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var products = await _productServiceClient.GetProductsAsync(token);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await _productServiceClient.CreateProductAsync(productDto, token);
        return CreatedAtAction(nameof(GetProductDetails), new { id = productDto.ProductId }, productDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await _productServiceClient.UpdateProductAsync(id, productDto, token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await _productServiceClient.DeleteProductAsync(id, token);
        return NoContent();
    }
}
