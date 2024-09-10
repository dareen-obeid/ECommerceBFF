using ECommerceBFF.DTOs;
using Newtonsoft.Json;

namespace ECommerceBFF.ServiceClient
{
    public class OrderServiceClient
    {
        private readonly HttpClient _httpClient;

        public OrderServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("OrderServiceClient");
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Orders/{id}");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<OrderDto>(await response.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var response = await _httpClient.GetAsync($"api/Orders/customer/{customerId}");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(await response.Content.ReadAsStringAsync());
        }

        public async Task CreateOrderAsync(OrderDto orderDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(orderDto), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Orders", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateOrderAsync(int id, OrderDto orderDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(orderDto), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Orders/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task CancelOrderAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Orders/cancel/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
