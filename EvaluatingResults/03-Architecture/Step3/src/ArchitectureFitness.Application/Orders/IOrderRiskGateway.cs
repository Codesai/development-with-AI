using ArchitectureFitness.Domain.Order;

namespace ArchitectureFitness.Application.Orders;

public interface IOrderRiskGateway
{
    Task<OrderRiskAssessment> GetRiskAssessment(string orderId, CancellationToken cancellationToken = default);
}
