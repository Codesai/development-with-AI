using ArchitectureFitness.Infrastructure.Orders;

namespace ArchitectureFitness.Domain.Order;

public sealed record Order(string Id)
{
    private readonly HttpOrderRiskGateway _riskGateway = new();

    public Task<bool> IsRisky()
    {
        return _riskGateway.IsRisky(Id);
    }
}
