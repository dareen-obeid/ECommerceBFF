using System.Threading.Tasks;
using ECommerceBFF.DTOs;
using ECommerceBFF.ServiceClient;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBFF.Controllers
{
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
            var product = await _productServiceClient.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productServiceClient.GetProductsAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            await _productServiceClient.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductDetails), new { id = productDto.ProductId }, productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            await _productServiceClient.UpdateProductAsync(id, productDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productServiceClient.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
