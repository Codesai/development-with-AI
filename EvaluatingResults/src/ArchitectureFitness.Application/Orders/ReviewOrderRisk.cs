using ArchitectureFitness.Domain.Order;

namespace ArchitectureFitness.Application.Orders;

public sealed class ReviewOrderRisk
{
    private readonly OrderRiskPolicy _riskPolicy;

    public ReviewOrderRisk(OrderRiskPolicy riskPolicy)
    {
        _riskPolicy = riskPolicy;
    }

    public Task<bool> Execute(string orderId, CancellationToken cancellationToken = default)
    {
        return _riskPolicy.IsRisky(orderId, cancellationToken);
    }
}
