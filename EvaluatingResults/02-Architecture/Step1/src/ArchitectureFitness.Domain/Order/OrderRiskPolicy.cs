using HttpOrderRiskGateway = ArchitectureFitness.Infrastructure.Orders.HttpOrderRiskGateway;

namespace ArchitectureFitness.Domain.Order;

public sealed class OrderRiskPolicy
{
    private readonly Order _order;
    private readonly HttpOrderRiskGateway _riskGateway = new();

    public OrderRiskPolicy(Order order)
    {
        _order = order;
    }

    public async Task<bool> IsRisky()
    {
        return await _riskGateway.IsRisky(_order.Id);
    }
}
