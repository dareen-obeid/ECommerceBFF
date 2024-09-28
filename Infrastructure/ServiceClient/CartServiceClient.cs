using System.Net.Http.Headers;
using System.Text;
using ECommerceBFF.DTOs;
using Newtonsoft.Json;

public class CartServiceClient
{
    private readonly HttpClient _httpClient;

    public CartServiceClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CartServiceClient");
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
    public async Task<CartDto> GetCartByCustomerIdAsync(int customerId, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = await _httpClient.GetAsync($"api/Cart/customer/{customerId}");
        return await ProcessResponse<CartDto>(response);
    }

    public async Task<IEnumerable<CartItemDto>> GetCartItemsByCartIdAsync(int cartId, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = await _httpClient.GetAsync($"api/Cart/items/{cartId}");
        return await ProcessResponse<IEnumerable<CartItemDto>>(response);
    }

    public async Task AddItemToCartAsync(CartItemDto cartItemDto, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var content = new StringContent(JsonConvert.SerializeObject(cartItemDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Cart/add", content);
        await ProcessResponse<CartItemDto>(response);
    }

    public async Task UpdateCartItemAsync(int cartItemId, CartItemDto cartItemDto, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var content = new StringContent(JsonConvert.SerializeObject(cartItemDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"api/Cart/update/{cartItemId}", content);
        await ProcessResponse<CartItemDto>(response);
    }

    public async Task RemoveCartItemAsync(int cartItemId, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = await _httpClient.DeleteAsync($"api/Cart/removeCartItem/{cartItemId}");
        await ProcessResponse<CartItemDto>(response);
    }
}
