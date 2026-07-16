using ArchitectureFitness.Domain.Order;

namespace ArchitectureFitness.Application.Orders;

public sealed class ReviewOrderRisk
{
    private readonly IOrderRiskGateway _riskGateway;

    public ReviewOrderRisk(IOrderRiskGateway riskGateway)
    {
        _riskGateway = riskGateway;
    }

    public async Task<bool> Execute(string orderId, CancellationToken cancellationToken = default)
    {
        var order = new Order(orderId);
        var riskPolicy = new OrderRiskPolicy(order);
        var assessment = await _riskGateway.GetRiskAssessment(order.Id, cancellationToken);

        return riskPolicy.IsRisky(assessment);
    }
}
