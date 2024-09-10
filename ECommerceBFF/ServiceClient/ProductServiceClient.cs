using ECommerceBFF.DTOs;
using Newtonsoft.Json;

namespace ECommerceBFF.ServiceClient
{
    public class ProductServiceClient
    {
        private readonly HttpClient _httpClient;

        public ProductServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ProductServiceClient");
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Product/{id}");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<ProductDto>(await response.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("api/Product");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(await response.Content.ReadAsStringAsync());
        }

        public async Task CreateProductAsync(ProductDto productDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(productDto), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Product", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProductAsync(int id, ProductDto productDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(productDto), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Product/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Product/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
