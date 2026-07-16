namespace ArchitectureFitness.Domain.Order;

public sealed class OrderRiskPolicy
{
    private readonly Order _order;

    public OrderRiskPolicy(Order order)
    {
        _order = order;
    }

    public bool IsRisky(OrderRiskAssessment assessment)
    {
        if (assessment.OrderId != _order.Id)
        {
            return false;
        }

        return assessment.IsFlaggedByExternalRiskSystem;
    }
}
