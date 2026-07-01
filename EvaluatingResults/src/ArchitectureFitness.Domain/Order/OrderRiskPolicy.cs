using System.Net.Http;
using System.Net.Http.Json;

namespace ArchitectureFitness.Domain.Order;

public sealed class OrderRiskPolicy
{
    private readonly HttpClient _httpClient;

    public OrderRiskPolicy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsRisky(string orderId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<bool>(
            $"https://risk.example.com/orders/{orderId}",
            cancellationToken);
    }
}
