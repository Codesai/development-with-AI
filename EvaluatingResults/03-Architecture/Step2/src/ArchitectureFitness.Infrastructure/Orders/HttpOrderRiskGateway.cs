using System.Net.Http.Json;

namespace ArchitectureFitness.Infrastructure.Orders;

public sealed class HttpOrderRiskGateway
{
    private readonly HttpClient _httpClient;

    public HttpOrderRiskGateway()
        : this(new HttpClient())
    {
    }

    public HttpOrderRiskGateway(HttpClient httpClient)
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
