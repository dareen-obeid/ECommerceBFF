using ECommerceBFF.DTOs;
using Newtonsoft.Json;

namespace ECommerceBFF.ServiceClient
{
    public class CartServiceClient
    {
        private readonly HttpClient _httpClient;

        public CartServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CartServiceClient");
        }

        public async Task<CartDto> GetCartByCustomerIdAsync(int customerId)
        {
            var response = await _httpClient.GetAsync($"api/Cart/customer/{customerId}");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<CartDto>(await response.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsByCartIdAsync(int cartId)
        {
            var response = await _httpClient.GetAsync($"api/Cart/items/{cartId}");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<IEnumerable<CartItemDto>>(await response.Content.ReadAsStringAsync());
        }

        public async Task AddItemToCartAsync(CartItemDto cartItemDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(cartItemDto), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Cart/add", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCartItemAsync(int cartItemId, CartItemDto cartItemDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(cartItemDto), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Cart/update/{cartItemId}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var response = await _httpClient.DeleteAsync($"api/Cart/removeCartItem/{cartItemId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCartAsync(int cartId)
        {
            var response = await _httpClient.DeleteAsync($"api/Cart/removeCart/{cartId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
