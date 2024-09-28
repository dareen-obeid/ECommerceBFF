using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ECommerceBFF.DTOs;
using System.Net.Http;

public class OrderServiceClient
{
    private readonly HttpClient _httpClient;

    public OrderServiceClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("OrderServiceClient");
    }

    private async Task<T> ProcessResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            // Try to deserialize the error response
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(content);
                throw new HttpRequestException($"Service Error: {errorResponse?.Message ?? "Unknown error"}");
            }
            catch (JsonException) // Catch JSON errors in case the error format is unexpected
            {
                throw new HttpRequestException("An error occurred while processing the response.");
            }
        }

        return JsonConvert.DeserializeObject<T>(content);
    }


    public async Task<OrderDto> GetOrderByIdAsync(int id, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = await _httpClient.GetAsync($"api/Orders/{id}");
        return await ProcessResponse<OrderDto>(response);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(int customerId, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = await _httpClient.GetAsync($"api/Orders/customer/{customerId}");
        return await ProcessResponse<IEnumerable<OrderDto>>(response);
    }

    public async Task CreateOrderAsync(OrderDto orderDto, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var content = new StringContent(JsonConvert.SerializeObject(orderDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Orders", content);
        await ProcessResponse<OrderDto>(response);
    }

    public async Task UpdateOrderAsync(int id, OrderDto orderDto, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var content = new StringContent(JsonConvert.SerializeObject(orderDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"api/Orders/{id}", content);
        await ProcessResponse<OrderDto>(response);
    }

    public async Task CancelOrderAsync(int id, string jwtToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = await _httpClient.DeleteAsync($"api/Orders/cancel/{id}");
        await ProcessResponse<OrderDto>(response);
    }
}
