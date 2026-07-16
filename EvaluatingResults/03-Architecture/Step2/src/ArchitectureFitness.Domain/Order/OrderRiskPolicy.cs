using ArchitectureFitness.Infrastructure.Orders;

namespace ArchitectureFitness.Domain.Order;

public sealed class OrderRiskPolicy
{
    private readonly HttpOrderRiskGateway _riskGateway;

    public OrderRiskPolicy(HttpOrderRiskGateway riskGateway)
    {
        _riskGateway = riskGateway;
    }

    public async Task<bool> IsRisky(string orderId, CancellationToken cancellationToken = default)
    {
        return await _riskGateway.IsRisky(orderId, cancellationToken);
    }
}
