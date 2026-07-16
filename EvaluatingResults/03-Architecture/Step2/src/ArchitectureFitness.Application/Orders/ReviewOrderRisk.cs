using ArchitectureFitness.Domain.Order;

namespace ArchitectureFitness.Application.Orders;

public sealed class ReviewOrderRisk
{
    public Task<bool> Execute(string orderId, CancellationToken cancellationToken = default)
    {
        var order = new Order(orderId);
        var riskPolicy = new OrderRiskPolicy(order);

        return riskPolicy.IsRisky(cancellationToken);
    }
}
