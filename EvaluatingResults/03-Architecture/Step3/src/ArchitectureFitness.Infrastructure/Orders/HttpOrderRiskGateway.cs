using System.Net.Http.Json;
using ArchitectureFitness.Application.Orders;
using ArchitectureFitness.Domain.Order;

namespace ArchitectureFitness.Infrastructure.Orders;

public sealed class HttpOrderRiskGateway : IOrderRiskGateway
{
    private readonly HttpClient _httpClient;

    public HttpOrderRiskGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OrderRiskAssessment> GetRiskAssessment(
        string orderId,
        CancellationToken cancellationToken = default)
    {
        var isRisky = await _httpClient.GetFromJsonAsync<bool>(
            $"https://risk.example.com/orders/{orderId}",
            cancellationToken);

        return new OrderRiskAssessment(orderId, isRisky);
    }
}
