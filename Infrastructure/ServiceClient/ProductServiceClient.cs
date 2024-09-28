using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ECommerceBFF.DTOs;

public class ProductServiceClient
{
    private readonly HttpClient _httpClient;

    public ProductServiceClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ProductServiceClient");
    }

    private async Task<T> ProcessResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(content);
            throw new ApplicationException($"Service Error: {errorResponse?.Message ?? "No error message provided."}");
        }
        return JsonConvert.DeserializeObject<T>(content);
    }

    // Send JWT token along with each request
    public async Task<ProductDto> GetProductByIdAsync(int id, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = await _httpClient.GetAsync($"api/Product/{id}");
        return await ProcessResponse<ProductDto>(response);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = await _httpClient.GetAsync("api/Product");
        return await ProcessResponse<IEnumerable<ProductDto>>(response);
    }

    public async Task CreateProductAsync(ProductDto productDto, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Product", content);
        await ProcessResponse<ProductDto>(response);
    }

    public async Task UpdateProductAsync(int id, ProductDto productDto, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"api/Product/{id}", content);
        await ProcessResponse<ProductDto>(response);
    }

    public async Task DeleteProductAsync(int id, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = await _httpClient.DeleteAsync($"api/Product/{id}");
        await ProcessResponse<ProductDto>(response);
    }
}
