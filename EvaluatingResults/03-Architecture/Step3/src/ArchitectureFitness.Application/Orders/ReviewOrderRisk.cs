using ArchitectureFitness.Domain.Order;

namespace ArchitectureFitness.Application.Orders;

public sealed class ReviewOrderRisk
{
    private readonly IOrderRiskGateway _riskGateway;
    private readonly OrderRiskPolicy _riskPolicy;

    public ReviewOrderRisk(IOrderRiskGateway riskGateway, OrderRiskPolicy riskPolicy)
    {
        _riskGateway = riskGateway;
        _riskPolicy = riskPolicy;
    }

    public async Task<bool> Execute(string orderId, CancellationToken cancellationToken = default)
    {
        var assessment = await _riskGateway.GetRiskAssessment(orderId, cancellationToken);

        return _riskPolicy.IsRisky(assessment);
    }
}
